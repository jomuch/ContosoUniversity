using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Range(0, 5)]
        public int Credits { get; set; }

        public int DepartmentID { get; set; }

        public Department? Department { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
    }
}
