using Microsoft.EntityFrameworkCore;

namespace DBWebAPI.Models.CodeFirst
{
    public class OrgCodeFirstContext : DbContext
    {
        public OrgCodeFirstContext(DbContextOptions<OrgCodeFirstContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }
    }
}
