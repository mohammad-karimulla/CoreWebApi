using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBWebAPI.Models.CodeFirst
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string EmployeeName { get; set; }
        
        [Column(TypeName = "varchar(500)")]
        public string Department { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? DateOfJoining { get; set; }
        
        [Column(TypeName = "varchar(500)")]
        public string PhotoFileName { get; set; }
    }
}
