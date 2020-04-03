using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public class MockDbService : IDbService
    {
        List<Student> students = new List<Student>();
        public IEnumerable<Student> GetStudents()
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18289;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT s.firstname, s.lastname, s.birthdate, study.name, e.semester " +
                                  "FROM Enrollment e " +
                                  "JOIN student s ON e.idenrollment = s.idenrollment " +
                                  "JOIN studies study ON e.idstudy = study.idstudy " +
                                  "ORDER BY s.firstname;";
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.Study = dr["Name"].ToString();
                    st.Semester = dr["Semester"].ToString();
                    students.Add(st);
                }
            }
            return students;
        }

        public List<string> GetStudentsEntries(int id)
        {
            List<string> entries = new List<string>();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18289;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT e.semester " +
                                  "FROM enrollment e " +
                                  "WHERE e.idenrollment = (SELECT s.idenrollment FROM student s " +
                                  $"WHERE s.indexnumber=@id)";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    string tmp = dr["Semester"].ToString();
                    entries.Add(tmp);
                }
            }
            return entries;
        }
    }
}