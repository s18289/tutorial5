using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs.Requests;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class SqlServerStudentDbService : ControllerBase, IStudentServiceDb
    {
        private readonly string connectionString = "Data Source=db-mssql;Initial Catalog=s18289;Integrated Security=True";
        private int idStudies, idEnrollment;
        private readonly DateTime date = DateTime.Now;
       

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

                if(request.Studies == null)
                {
                    return BadRequest("Studies == null");
                }

                //1. Check if studies exists -> 400
                var dr = com.ExecuteReader();
                if(dr.Read())
                {
                    idStudies = (int)dr["idStudy"];
                }
                else
                {
                    dr.Close();
                    transaction.Rollback();
                    return BadRequest("Studies does not exist!");
                }
                dr.Close();

                //2. Check if enrollment exists -> INSERT
                com.CommandText = "SELECT MAX(StartDate) FROM enrollment WHERE semester = 1 AND idStudy=@idStudies";
                com.Parameters.AddWithValue("idStudies", idStudies);
                dr = com.ExecuteReader();
                if(!dr.Read())
                {
                    dr.Close();
                }
                else
                {
                    dr.Close();
                    com.CommandText = "SELECT MAX(idEnrollment) 'idEnrollment' FROM enrollment";
                    dr = com.ExecuteReader();
                    dr.Read();
                    idEnrollment = (int)dr["idEnrollment"] + 1;
                    dr.Close();
                    com.CommandText = "INSERT INTO enrollment VALUES (@idEnrollment, 1, " + idStudies + ", '" + date + "')";
                    com.Parameters.AddWithValue("idEnrollment", idEnrollment);
                    com.ExecuteNonQuery();
                }
                dr.Close();

                //3. Check if index does not exists -> INSERT/400
                com.CommandText = "SELECT * FROM student WHERE IndexNumber=@IndexNumber";
                com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                dr = com.ExecuteReader();
                if(dr.Read())
                {
                    dr.Close();
                    transaction.Rollback();
                    return BadRequest("Student with IndexNumber: " + request.IndexNumber + " already exists!");
                }
                else
                {
                    dr.Close();
                    com.CommandText = "INSERT INTO student VALUES (@sNumber, @FirstName, @LastName, @BirthDate, @idEnroll)";
                    com.Parameters.AddWithValue("sNumber", request.IndexNumber);
                    com.Parameters.AddWithValue("FirstName", request.FirstName);
                    com.Parameters.AddWithValue("LastName", request.LastName);
                    com.Parameters.AddWithValue("BirthDate", request.BirthDate);
                    com.Parameters.AddWithValue("idEnroll", idEnrollment);
                    com.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            //return
            return Ok("New student has been enrolled to study\n" +
                      "IndexNumber: " + request.IndexNumber + "\n" +
                      "Name: " + request.FirstName + "\n" +
                      "Surname: " + request.LastName + "\n" +
                      "BirthDate: " + request.BirthDate + "\n" +
                      "Studies: " + request.Studies);
        }

        public IActionResult PromoteStudents(Enrollment enrollment)
        {


            
            //return
            return Ok();
        }
    }
}