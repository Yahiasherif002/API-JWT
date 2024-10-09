using API_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            List<Department> departments = _context.Departments.ToList();

            return Ok(departments);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetDepartmentById(int id)
        {
            var department = _context.Departments.FirstOrDefault(d=>d.Id == id);

            return Ok(department);
        }



        [HttpPost]
        [HttpPost]
        public IActionResult AddDepartment(Department department)
        {
            if (department == null)
                return BadRequest("Department cannot be null.");

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Departments.Add(department);
                    _context.SaveChanges();

                    // Commit the transaction only if everything is successful
                    

                    var result= CreatedAtAction("GetDepartmentById", new { id = department.Id }, department);
                    
                    transaction.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    // Rollback transaction in case of an error
                    transaction.Rollback();
                    return StatusCode(500, "An error occurred: " + ex.Message);
                }
            }
        }

    }
}

