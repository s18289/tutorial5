using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs.Requests;

namespace WebApplication1.Services
{
    public class SqlServerStudentDbService : ControllerBase, IStudentServiceDb
    {
        private readonly string connectionString = "Data Source=db-mssql;Initial Catalog=s18289;Integrated Security=True";
        private int idStudies, idEnrollment;
        private DateTime date = DateTime.Now;

        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection(connectionString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM studies WHERE Name=@Name";
                com.Parameters.AddWithValue("Name", request.Studies);
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                com.Transaction = transaction;

                //1. Check if studies exists -> 404
                var dr = com.ExecuteReader();
                if(dr.Read())
                {
                    idStudies = (int)dr["idStudy"];
                }
                else
                {
                    transaction.Rollback();
                    return BadRequest("Studies does not exist!");
                }
                dr.Close();

                //2. Check if enrollment exists -> INSERT
                com.CommandText = "SELECT MAX(StartDate) FROM enrollment WHERE semester = 1 AND idStudy=@idStudies";
                com.Parameters.AddWithValue("idStudies", idStudies);
                dr = com.ExecuteReader();
                if(dr.Read())
                {
                    dr.Close();
                }
                else
                {
                    com.CommandText = "SELECT MAX(idEnrollment) 'idEnrollment' FROM enrollment";
                    dr = com.ExecuteReader();
                    dr.Read();
                    idEnrollment = (int)dr["idEnrollment"] + 1;
                    dr.Close();
                    com.CommandText = "INSERT INTO enrollment VALUES (@idEnrollment, 1, idStudies, date)";
                    com.Parameters.AddWithValue("idEnrollment", idEnrollment);
                    com.ExecuteNonQuery();
                }

                //3. Check if index does not exists -> INSERT/400
                com.CommandText = "SELECT * FROM student WHERE IndexNumber=@IndexNumber";
                com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                dr = com.ExecuteReader();
                if(dr.Read())
                {
                    transaction.Rollback();
                    return BadRequest("Student with IndexNumber: " + request.IndexNumber + " already exists!");
                }
                else
                {
                    dr.Close();
                    com.CommandText = "INSERT INTO student VALUES (@IndexNumber, @FirstName, @LastName, @BirthDate, @idEnrollment)";
                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                    com.Parameters.AddWithValue("FirstName", request.FirstName);
                    com.Parameters.AddWithValue("LastName", request.LastName);
                    com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                    com.ExecuteNonQuery();
                }
                transaction.Commit();
                //4. return Enrollment model
                return Ok();
            }
        }

        public IActionResult PromoteStudents(int semester, string studies)
        {
            return Ok();
        }
    }
}
