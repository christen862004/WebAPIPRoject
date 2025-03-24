using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIPRoject.DTO;
using WebAPIPRoject.Models;

namespace WebAPIPRoject.Controllers
{
    [Route("api/[controller]")]//api/Department (req Vereb)
    [ApiController]//chage binding - validation

    public class DepartmentController : ControllerBase
    {
        private readonly ITIContext context;

        public DepartmentController(ITIContext context)
        {
            this.context = context;
        }
        [HttpGet("c/{id:int}")]
        public IActionResult DeptWithCount(int id)
        {
            var dept=
                context.Departments.Include(d=>d.Employees)
                .Select(d=>new DeprtmentWithCountDTO() 
                    { DeptId=d.Id,DEptName=d.Name,EmpCount=d.Employees.Count()})
                .FirstOrDefault(d=>d.DeptId==id);
            return Ok(dept);
        }
        
        
        
        [HttpGet]//api/department Get
        public IActionResult getAll()
        {
            List<Department> deptList = context.Departments.ToList();
            return Ok(deptList);
        }

        [HttpGet]
        [Route("{id:int}")]//api/departmnet/1
        public IActionResult getbyID(int id)
        {
            Department dept = context.Departments.FirstOrDefault(d=>d.Id==id);
            if (dept != null)
                return Ok(dept);//staus code 200 ,dept response body
            else
                return NotFound();
        }

        [HttpGet("{name:alpha}")]//api/department/ahmed
        public IActionResult GetByName(string name)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Name.Contains(name));
            if (dept != null)
                return Ok(dept);//staus code 200 ,dept response body
            else
                return NotFound();
        }

        [HttpGet("m/{name:alpha}")]//api/department/mg/ahmed
        public IActionResult GetByManagerName(string name)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.ManagerName.Contains(name));
            if (dept != null)
                return Ok(dept);//staus code 200 ,dept response body
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult insert(Department dept)//reqq body
        {
            if(ModelState.IsValid)
            {
                try
                {
                    context.Departments.Add(dept);
                    context.SaveChanges();
                    //return Created()$"http://localhost:7960/api/Department/{dept.Id}", dept);   //Ok("Created");
                    return CreatedAtAction("getbyID", new { id = dept.Id }, dept);

                }catch (Exception ex)
                {
                    ModelState.AddModelError("",ex.InnerException.Message);
                }
            }
            return BadRequest(ModelState);
        }
        //[HttpPatch]//update at soe filed
        
        
        [HttpPut("{id:int}")]//api/depat,ment/id
        public IActionResult Edit(int id,Department deptFromReq)
        {
            if (ModelState.IsValid)
            {
                Department deptFromDb= context.Departments.FirstOrDefault(d => d.Id == id);
                if(deptFromDb != null)
                {
                    deptFromDb.Name = deptFromReq.Name;
                    deptFromDb.ManagerName = deptFromReq.ManagerName;
                    context.SaveChanges();
                    return NoContent();//Ok("Updated");
                }
                ModelState.AddModelError("", "Invalid id");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")] //api/department/1   (delete)
        public IActionResult Delete(int id)
        {
            Department deptFromDb = context.Departments.FirstOrDefault(d => d.Id == id);
            if (deptFromDb != null)
            {
                context.Departments.Remove(deptFromDb);
                context.SaveChanges();
                return NoContent(); //Ok("Deleted");
            }
            return BadRequest("Invalid ID");
        }
        
    }
}
