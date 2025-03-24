using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIPRoject.Models;

namespace WebAPIPRoject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]//1) custom bing 
    public class BindController : ControllerBase
    {
        private readonly ITIContext context;

        public BindController(ITIContext context)
        {
            this.context = context;
        }
        [HttpGet("{id:int}")]//api/departmnet/1
        public ActionResult<GeneralResponse> getbyID(int id)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Id == id);
            GeneralResponse repsonse = new GeneralResponse();
            if (dept != null)
            {
                repsonse.IsPass = true;
                repsonse.Data = dept;
                return repsonse;//Ok(dept);//staus code 200 ,dept response body
            }
            else
            {
                repsonse.IsPass = false;
                repsonse.Data = "Id Not Found";
                return repsonse;
            }
        }



        //[HttpGet("{id:int}")]
        //public IActionResult testPrimitive(int id, string name)
        //{
        //    return Ok();
        //}

        [HttpPut("{id:int}")]
        public IActionResult TestComplex(int id,[FromBody]string name)//,Department dept)
        {
            return Ok();
        }
        //custimze 
        
        
        [HttpGet("{lang}/{latit}")]//api/bind/21123.12312/13123,32312
        public IActionResult GetLocation([FromRoute]Location Loc)//default form body
        {
            return Ok();
        }
    }
}
