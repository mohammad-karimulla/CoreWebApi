using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.DataFirst
{
    public partial class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string Department { get; set; }

        public DateTime DateOfJoining { get; set; }

        public string PhotoFileName { get; set; }

        public virtual Department DepartmentNavigation { get; set; }
    }
}
