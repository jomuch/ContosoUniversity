using ContosoUniversity.Models;
using System.Collections.Generic;

namespace ContosoUniversity.Services
{
    public interface ICourseService
    {
        IEnumerable<Course> GetAllCourses();
        Course? GetCourseById(int id);
    }
}
