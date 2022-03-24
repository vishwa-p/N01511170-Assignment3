using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using N01511170_Assignment3.Models;
using BlogProject.Models;
namespace N01511170_Assignment3.Controllers
{
    public class StudentDataController : ApiController
    {
        // This database context class allows us to access and get from SchoolDb database
        private BlogDbContext Blog = new BlogDbContext();


        /// StudentData Controller will access students table from SchoolDb Database
        ///  <returns>
        /// It returns a list of students from table.(StudentId,StudentFname,StudentLname,StudentEnrolDate,StudentNumber
        /// </returns>
        /// GET api/StudentData/ListStudents
        [HttpGet]
        public IEnumerable<Student> ListStudents()
        {
            //Creating an instance of a connection
            MySql.Data.MySqlClient.MySqlConnection Conn = Blog.AccessDatabase();

            //Opening connection between the web server and database
            Conn.Open();

            //Establish a new query for our database
            MySql.Data.MySqlClient.MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from students";

            //Collecting Result Set of Query into a variable
            MySql.Data.MySqlClient.MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Creating an empty list of Students' Information
            List<Student> Students = new List<Student> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                DateTime StudentEnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);


                Student NewStudent = new Student();
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.StudentEnrolDate = StudentEnrolDate;

                //Add the Students' Data to the List
                Students.Add(NewStudent);
            }

            //Closing the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of Students' information
            return Students;
        }

        /// <summary>
        /// To Find out a teacher from given databased by given ID
        /// </summary>
        /// <param name="id">The student primary key</param>
        /// <returns>An student object</returns>
        [HttpGet]
        [Route("api/studentdata/findstudent/{studentid}")]
        public Student FindStudent(int studentid)
        {
            Student NewStudent = new Student();

            //Creating an instance of a connection
            MySql.Data.MySqlClient.MySqlConnection Conn = Blog.AccessDatabase();

            //Opening connection between the web server and database
            Conn.Open();

            //Establish a new query for our database
            MySql.Data.MySqlClient.MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from students where studentid = " + studentid;

            //Collecting Result Set of Query into a variable
            MySql.Data.MySqlClient.MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                DateTime StudentEnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.StudentEnrolDate = StudentEnrolDate;

            }
            return NewStudent;
        }

    }
}
