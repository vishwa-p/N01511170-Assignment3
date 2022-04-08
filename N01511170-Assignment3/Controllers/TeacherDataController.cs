using BlogProject.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using N01511170_Assignment3.Models;
using MySql.Data.MySqlClient;

namespace N01511170_Assignment3.Controllers
{
    public class TeacherDataController : ApiController
    {
       
        // This database context class allows us to access and get from SchoolDb database
        private BlogDbContext Blog = new BlogDbContext();

        /// A list of authors (first names and last names)
        /// TeacherData Controller will access teachers table from SchoolDb Database
        ///  <returns>
        /// It returns a list of teachers from table.(TeacherId,TeacherFname,TeacherLname,TeacherHiredate,TeacherSalary
        /// </returns>
        /// GET api/TeacherData/ListTeachers
        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey = null)
        {
            //Creating an instance of a connection
            MySql.Data.MySqlClient.MySqlConnection Conn = Blog.AccessDatabase();

            //Opening connection between the web server and database
            Conn.Open();

            //Establish a new query for our database
            MySql.Data.MySqlClient.MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            /*cmd.CommandText = "Select * from teachers";*/
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            //Collecting Result Set of Query into a variable
            MySql.Data.MySqlClient.MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Creating an empty list of Teachers' Information
            List<Teacher> Teachers = new List<Teacher> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string TeacherNumber = ResultSet["employeenumber"].ToString();
                DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                Decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);

                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherNumber = TeacherNumber;
                NewTeacher.TeacherHireDate = TeacherHireDate;
                NewTeacher.TeacherSalary = TeacherSalary;
                //Add the teachers' data to the List
                Teachers.Add(NewTeacher);
            }

            //Closing the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of Teachers' information
            return Teachers;

        }

        /// <summary>
        /// To Find out a teacher from given databased by given ID
        /// </summary>
        /// <param name="id">The teacher primary key</param>
        /// <returns>An teacher object</returns>
            [HttpGet]
            [Route("api/teacherdata/findteacher/{teacherid}")]
        public Teacher FindTeacher(int teacherid)
        {
            Teacher NewTeacher = new Teacher();

            //Creating an instance of a connection
            MySql.Data.MySqlClient.MySqlConnection Conn = Blog.AccessDatabase();

            //Opening connection between the web server and database
            Conn.Open();

            //Establish a new query for our database
            MySql.Data.MySqlClient.MySqlCommand cmd = Conn.CreateCommand();

          
            //SQL QUERY
            /*cmd.CommandText = "Select * from Teachers where teacherid = " + teacherid;*/
            cmd.CommandText = "Select * from Teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", teacherid);
            cmd.Prepare();

            //Collecting Result Set of Query into a variable
            MySql.Data.MySqlClient.MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string TeacherNumber = ResultSet["employeenumber"].ToString();
                DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                Decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);

                NewTeacher.TeacherId = TeacherId;
                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.TeacherNumber = TeacherNumber;
                NewTeacher.TeacherHireDate = TeacherHireDate;
                NewTeacher.TeacherSalary = TeacherSalary;
            }
            return NewTeacher;
        }
        /// <summary>
        /// Deletes an teacher from the connected MySQL Database if the ID of that author exists.
        /// </summary>
        /// <param name="id">The ID of the teacher.</param>
        /// <example>POST /api/TeacherrData/Deleteteacher/7</example>
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            //Create an instance of a connection
            MySql.Data.MySqlClient.MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "DELETE teachers , classes  FROM teachers  INNER JOIN classes WHERE teachers.teacherId = classes.teacherId and teachers.teacherId =@id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            Conn.Close();
        }

        [HttpPost]
        [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Blog.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into teachers " +
                "(teacherfname, teacherlname, employeenumber, hiredate, salary)" +
                " values (@TeacherFname,@TeacherLname,@TeacherNumber, @TeacherHireDate, @TeacherSalary)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@TeacherNumber", NewTeacher.TeacherNumber);
            cmd.Parameters.AddWithValue("@TeacherHireDate", NewTeacher.TeacherHireDate);
            cmd.Parameters.AddWithValue("@TeacherSalary", NewTeacher.TeacherSalary);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            Conn.Close();
        }
    }
}
