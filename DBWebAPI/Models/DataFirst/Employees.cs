using System;

namespace DBWebAPI.Models.DataFirst
{
    public partial class Employees
    {
        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }
        
        public int Department { get; set; }
        
        public DateTime DateOfJoining { get; set; }
        
        public string PhotoFileName { get; set; }

        public virtual Departments DepartmentNavigation { get; set; }
    }
}
