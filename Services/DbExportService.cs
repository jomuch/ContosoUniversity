using ContosoUniversity.Data;
using ContosoUniversity.Models;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace ContosoUniversity.Services
{
    public class DbExportService : ICourseService
    {
        private readonly SchoolContext _context;

        public DbExportService(SchoolContext context)
        {
            _context = context;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }

        public Course? GetCourseById(int id)
        {
            return _context.Courses.FirstOrDefault(c => c.CourseID == id);
        }

        public XDocument ExportToXml()
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
