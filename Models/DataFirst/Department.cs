using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DataFirst
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
