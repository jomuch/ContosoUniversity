using ContosoUniversity.Data;
using ContosoUniversity.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ContosoUniversity.Services
{
    // Handles both courses and students
    public interface IDataExportService
    {
        IEnumerable<Course> GetAllCourses();
        Course? GetCourseById(int id);
        IEnumerable<Student> GetAllStudents();
        Student? GetStudentById(int id);
        XDocument ExportCoursesToXml();
        XDocument ExportStudentsToXml();
    }

    public class DbExportService : IDataExportService
    {
        private readonly SchoolContext _context;

        public DbExportService(SchoolContext context)
        {
            _context = context;
        }

        // Courses
        public IEnumerable<Course> GetAllCourses() => _context.Courses.ToList();
        public Course? GetCourseById(int id) => _context.Courses.FirstOrDefault(c => c.CourseID == id);

        // Students
        public IEnumerable<Student> GetAllStudents() => _context.Students.ToList();
        public Student? GetStudentById(int id) => _context.Students.FirstOrDefault(s => s.ID == id);

        // Export courses to XML
        public XDocument ExportCoursesToXml()
        {
            var courses = _context.Courses.Select(c => new XElement("course",
                new XElement("CourseID", c.CourseID),
                new XElement("Title", c.Title),
                new XElement("Credits", c.Credits),
                new XElement("DepartmentID", c.DepartmentID)
            ));

            return new XDocument(
                new XElement("School",
                    new XElement("Courses", courses)
                )
            );
        }

        // Export students to XML
        public XDocument ExportStudentsToXml()
        {
            var students = _context.Students.Select(s => new XElement("student",
                new XElement("ID", s.ID),
                new XElement("FirstMidName", s.FirstMidName),
                new XElement("LastName", s.LastName),
                new XElement("EnrollmentDate", s.EnrollmentDate.ToString("yyyy-MM-dd"))
            ));

            return new XDocument(
                new XElement("School",
                    new XElement("Students", students)
                )
            );
        }
    }
}
