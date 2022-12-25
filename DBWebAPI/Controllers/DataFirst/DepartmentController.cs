using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBWebAPI.Models.DataFirst;
using System;

namespace WebAPI.Controllers.DataFirst
{
    [Route("api/datafirst/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly OrgDataFirstContext _context;

        public DepartmentController(OrgDataFirstContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departments>>> GetDepartments()
        {
            return await _context.Departments.OrderBy(d => d.DepartmentID).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Departments>> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        [HttpPut]
        public async Task<IActionResult> PutDepartment(Departments department)
        {
            _context.Entry(department).State = EntityState.Modified;

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
        public async Task<IActionResult> PostDepartment(Departments department)
        {
            try
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                return new JsonResult("DB First: Added Successfully");
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound();
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return new JsonResult("DB First: Deleted Successfully");
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }

        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllDepartmentNames()
        {
            return await _context.Departments.Select(d => new { d.DepartmentName }).OrderBy(d => d.DepartmentName).ToListAsync();
        }
    }
}
