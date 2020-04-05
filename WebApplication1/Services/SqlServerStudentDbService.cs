using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs.Requests;

namespace WebApplication1.Services
{
    public class SqlServerStudentDbService : IStudentServiceDb
    {
        private readonly string connectionString = "Data Source=db-mssql;Initial Catalog=s18289;Integrated Security=True";

        public void EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = new SqlConnection(connectionString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM studies WHERE Name = @Name;";
                //com.Parameters.AddWithValue("Name", student.Studies);
                con.Open();
                SqlTransaction sqlTransaction = con.BeginTransaction();
                com.Transaction = sqlTransaction;
            }
            //write code here
            throw new NotImplementedException();
        }

        public void PromoteStudents(int semester, string studies)
        {
            //write code here
            throw new NotImplementedException();
        }
    }
}
