using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult GetEmployees()
        {
            string query = @"
                    SELECT EmployeeID, EmployeeName, Department, 
                    CONVERT(varchar(10), DateOfJoining, 120) as DateOfJoining,
                    PhotoFileName
                    FROM dbo.Employee
                    ";
            string sqlDataSource = _configuration.GetConnectionString("OrganizationAppCon");

            DataTable dataTable = new DataTable();

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    dataTable.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(dataTable);
        }

        [HttpPost]
        public JsonResult PostEmployee(Employee employee)
        {
            string query = @"
                    INSERT INTO dbo.Employee
                    (EmployeeName, Department, DateOfJoining, PhotoFileName)
                    VALUES 
                    (
                    '" + employee.EmployeeName + @"',
                    '" + employee.Department + @"',
                    '" + employee.DateOfJoining + @"',
                    '" + employee.PhotoFileName + @"'
                    )
                    ";
            string sqlDataSource = _configuration.GetConnectionString("OrganizationAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.ExecuteNonQuery();
                    myCon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult PutEmployee(Employee employee)
        {
            string query = @"
                    UPDATE dbo.Employee SET 
                    EmployeeName = '" + employee.EmployeeName + @"',
                    Department = '" + employee.Department+ @"',
                    DateOfJoining = '" + employee.DateOfJoining + @"',
                    PhotoFileName = '" + employee.PhotoFileName + @"'
                    WHERE EmployeeID = " + employee.EmployeeID + @"
                    ";
            string sqlDataSource = _configuration.GetConnectionString("OrganizationAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.ExecuteNonQuery();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{employeeID}")]
        public JsonResult DeleteDepartment(int employeeID)
        {
            string query = @"
                    DELETE FROM dbo.Employee
                    WHERE EmployeeID = " + employeeID + @"
                    ";
            string sqlDataSource = _configuration.GetConnectionString("OrganizationAppCon");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.ExecuteNonQuery();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}
