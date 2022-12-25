using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using DBWebAPI.Models.ADO;
using System;

namespace DBWebAPI.Controllers.ADO
{
    [Route("api/ado/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult GetDepartments()
        {
            try
            {
                string query = @"SELECT DepartmentID, DepartmentName FROM dbo.Department";
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
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult PostDepartment(Department department)
        {
            try
            {
                string query = @"
                    INSERT INTO dbo.Department VALUES 
                    ('" + department.DepartmentName + @"')";
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
        public JsonResult PutDepartment(Department department)
        {
            try
            {
                string query = @"
                    UPDATE dbo.Department SET 
                    DepartmentName = '" + department.DepartmentName + @"'
                    WHERE DepartmentID = " + department.DepartmentID + @"
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

        [HttpDelete("{departmentID}")]
        public JsonResult DeleteDepartment(int departmentID)
        {
            try
            {
                string query = @"
                    DELETE FROM dbo.Department
                    WHERE DepartmentID = " + departmentID + @"
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

                return new JsonResult("ADO: Deleted Successfully");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public JsonResult GetAllDepartmentNames()
        {
            try
            {
                string query = @"SELECT DepartmentName FROM dbo.Department";
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
    }
}
