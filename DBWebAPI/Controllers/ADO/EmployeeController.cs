using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using DBWebAPI.Models;
using DBWebAPI.Models.ADO;
using System;

namespace DBWebAPI.Controllers.ADO
{
    [Route("api/ado/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly IFileOperations _fileOperations;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env, IFileOperations fileOperations)
        {
            _configuration = configuration;
            _env = env;
            _fileOperations = fileOperations;
        }

        [HttpGet]
        public JsonResult GetEmployees()
        {
            try
            {
                string query = @"
                    SELECT EmployeeID, EmployeeName, Department, 
                    CONVERT(varchar(10), DateOfJoining, 120) as DateOfJoining,
                    PhotoFileName
                    FROM dbo.Employee
                    ORDER BY EmployeeID
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult PostEmployee(Employee employee)
        {
            try
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

                return new JsonResult("ADO: Added Successfully");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        public JsonResult PutEmployee(Employee employee)
        {
            try
            {
                string query = @"
                    UPDATE dbo.Employee SET 
                    EmployeeName = '" + employee.EmployeeName + @"',
                    Department = '" + employee.Department + @"',
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

                return new JsonResult("ADO: Updated Successfully");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("{employeeID}")]
        public JsonResult DeleteEmployee(int employeeID)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            return new JsonResult(_fileOperations.SaveImage(_env, Request));
        }
    }
}
