using ContosoUniversity.Models;

namespace ContosoUniversity.ViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; } = new List<Instructor>(); // <-- Check this line
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();
        public IEnumerable<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}