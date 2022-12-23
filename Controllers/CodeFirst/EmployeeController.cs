using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Models.CodeFirst;

namespace WebAPI.Controllers.CodeFirst
{
    [Route("api/codefirst/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly OrgCodeFirstContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IFileOperations _fileOperations;

        public EmployeeController(OrgCodeFirstContext context, IWebHostEnvironment env, IFileOperations fileOperations)
        {
            _context = context;
            _env = env;
            _fileOperations = fileOperations;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpPut]
        public async Task<IActionResult> PutEmployee(Employee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return new JsonResult("CodeFirst: Updated Successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return new JsonResult("CodeFirst: Added Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return new JsonResult("CodeFirst: Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            return new JsonResult(_fileOperations.SaveImage(_env, Request));
        }
    }
}
