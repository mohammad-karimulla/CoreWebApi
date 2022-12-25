using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DBWebAPI.Models.CodeFirst;
using System;

namespace WebAPI.Controllers.CodeFirst
{
    [Route("api/codefirst/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly OrgCodeFirstContext _context;

        public DepartmentController(OrgCodeFirstContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        [HttpPut]
        public async Task<IActionResult> PutDepartment(Department department)
        {
            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return new JsonResult("CodeFirst: Updated Successfully");
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                throw dbEx;
            }
            catch (Exception ex)
            {
                return new JsonResult($"Exception: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostDepartment(Department department)
        {
            try
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                return new JsonResult("CodeFirst: Added Successfully");
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

                return new JsonResult("CodeFirst: Deleted Successfully");
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
            return await _context.Departments.Select(d => new { d.DepartmentName }).ToListAsync();
        }
    }
}
