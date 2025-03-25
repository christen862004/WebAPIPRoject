using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIPRoject.DTO;
using WebAPIPRoject.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WebAPIPRoject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        [HttpPost("register")]//api/accoutn/register
        public async Task<IActionResult> Register(RegisterDto userFromConsumer) 
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName=userFromConsumer.UserName,
                    Email=userFromConsumer.Email
                };
                IdentityResult result= await userManager.CreateAsync(user, userFromConsumer.Password);
                if(result.Succeeded)
                {
                    //create token
                    return Ok("Account Create Success");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]//api/accuont/login
        public async Task<IActionResult> Login(LoginDto userFromConsumer)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user=await userManager.FindByNameAsync(userFromConsumer.UserName);
                if (user != null)
                {
                    bool found= await userManager.CheckPasswordAsync(user, userFromConsumer.Password);
                    if (found)
                    {
                        #region Create Token
                        string jti = Guid.NewGuid().ToString();
                        var userRoles=await  userManager.GetRolesAsync(user);


                        List<Claim> claim = new List<Claim>();
                        claim.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claim.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claim.Add(new Claim(JwtRegisteredClaimNames.Jti,jti));//optional
                        if(userRoles != null)
                        {
                            foreach (var role in userRoles)
                            {
                                claim.Add(new Claim(ClaimTypes.Role, role));
                            }
                        }
                        //-----------------------------------------------
                        SymmetricSecurityKey signinKey =
                            new(Encoding.UTF8.GetBytes(config["JWT:Key"]));//hjhjhjhjh

                        SigningCredentials signingCredentials=
                            new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);
                        
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: config["JWT:Iss"],//url service provider
                            audience: config["JWT:Aud"],//url service consumer
                            expires: DateTime.Now.AddHours(1),
                            claims:claim,
                            signingCredentials: signingCredentials
                            );//

                        //copact jhjh.kjk.lkl
                        return Ok(new
                        {
                            expired= DateTime.Now.AddHours(1),
                            token =new JwtSecurityTokenHandler().WriteToken(myToken)
                        });
                        #endregion
                    }
                }
                ModelState.AddModelError("", "Invalid Account");
            }
            return BadRequest(ModelState);
        }
    }
}
