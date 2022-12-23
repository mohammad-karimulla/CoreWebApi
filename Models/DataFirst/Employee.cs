using System;

namespace WebAPI.Models.DataFirst
{
    public partial class Employee
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int Department { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string PhotoFileName { get; set; }

        public virtual Department DepartmentNavigation { get; set; }
    }
}
