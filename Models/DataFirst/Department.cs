using System.Collections.Generic;

namespace WebAPI.Models.DataFirst
{
    public partial class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public int DepartmentID { get; set; }

        public string DepartmentName { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
