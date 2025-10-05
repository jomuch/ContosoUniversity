using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        [Required]
        public int CourseID { get; set; }

        [Required]
        public int StudentID { get; set; }

        // Navigation properties
        public Course Course { get; set; } = null!;
        public Student Student { get; set; } = null!;

        // Use enum instead of decimal
        public Grade? Grade { get; set; }
    }
}
