using ContosoUniversity.Models;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ContosoUniversity.Services
{
    public interface ICourseService
    {
        IEnumerable<Course> GetAllCourses();
        Course? GetCourseById(int id);
        XDocument ExportToXml();
    }
}
