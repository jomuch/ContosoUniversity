namespace ContosoUniversity.Services
{
    public class CourseService : ICourseService
    {
        private readonly List<Course> _courses;

        public CourseService() => _courses = new List<Course>
            {
                new Course { CourseID = 1, Title = "Chemistry" },
                new Course { CourseID = 2, Title = "Microeconomics" },
                new Course { CourseID = 3, Title = "Macroeconomics" }
            };

        public IEnumerable<Course> GetAllCourses() => _courses;

        public Course? GetCourseById(int id) => _courses.FirstOrDefault(c => c.CourseID == id);
    }
}
