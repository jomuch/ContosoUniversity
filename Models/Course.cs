using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Course
    {
        // This ensures CourseID is not auto-generated, as you're setting it manually.
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int CourseID { get; set; }  // Course number

        // Title validation - minimum of 3 characters, max of 50 characters.
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; } = null!;  // Title of the course (non-nullable)

        // Credits can range from 0 to 5.
        [Range(0, 5)]
        public int Credits { get; set; }  // Credits for the course

        // DepartmentID is a foreign key.
        public int DepartmentID { get; set; }  // Foreign Key for Department

        // Navigation property for the associated Department (non-nullable).
        public Department Department { get; set; } = null!;  // Department the course belongs to

        // Collection of enrollments for the course.
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        // Collection of course assignments for instructors teaching this course.
        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
    }
}
