namespace ContosoUniversity.ViewModels
{
    public class AssignedCourseData
    {
        public int CourseID { get; set; }
        public string Title { get; set; } = null!;
        public bool Assigned { get; set; }
    }
}