using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using WebAPI.Models.DataFirst;
using WebAPI.Models.Pagination;
using WebAPI.Repository;

namespace WebAPI.Controllers.DataFirst
{
    [Route("api/datafirst/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly OrgDataFirstContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IFileOperations _fileOperations;
        private IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, OrgDataFirstContext context, IWebHostEnvironment env, IFileOperations fileOperations)
        {
            _employeeRepository = employeeRepository;
            _context = context;
            _env = env;
            _fileOperations = fileOperations;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees([FromQuery] PagingParameters pagingParameters)
        {
            return await _employeeRepository.GetEmployees(pagingParameters);
        }
        
        [HttpGet("get-all-count")]
        public async Task<ActionResult<int>> GetEmployeesCount()
        {
            return await _employeeRepository.GetEmployeesCount();
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

                return new JsonResult("DB First: Updated Successfully");
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

            return new JsonResult("DB First: Added Successfully");
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

            return new JsonResult("DB First: Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            return new JsonResult(_fileOperations.SaveImage(_env, Request));
        }
    }
}
