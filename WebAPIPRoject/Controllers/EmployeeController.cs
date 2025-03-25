using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIPRoject.DTO;
using WebAPIPRoject.Models;

namespace WebAPIPRoject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        ITIContext context;
        public EmployeeController(ITIContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public IActionResult GetEmp(int id)
        {
            EmpDept? emplist = context.Employee
                .Include(e => e.Department)
                .Select(e=>new EmpDept() { Id=e.Id,EmpName=e.Name,DeptName=e.Department.Name})
                .FirstOrDefault(e => e.Id == id);
            return Ok(emplist);
        }

        [HttpPost]
        public ActionResult<GeneralResponse> addEmp(Employee emp)
        {
            if (ModelState.IsValid)
            {
                context.Employee.Add(emp);
                context.SaveChanges();
                return new GeneralResponse() { IsPass=true,Data=emp};
            }
            return  BadRequest(new GeneralResponse() { IsPass = false, Data = ModelState });
        }
    }
}
