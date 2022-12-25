using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBWebAPI.Models.CodeFirst
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string DepartmentName { get; set; }
    }
}
