using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    // The Course model class represents a course offered by the university.
    public class Course
    {
        // CourseID is the primary key. 
        // [DatabaseGenerated(DatabaseGeneratedOption.None)] prevents EF from treating it as an identity column (auto-increment).
        // This allows the application to specify the course number (e.g., 1050, 4024).
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int CourseID { get; set; } // <--- FIXES CS1061/CS0117 for CourseID

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(0, 5)]
        public int Credits { get; set; } // <--- FIXES CS1061/CS0117 for Credits

        // Foreign Key property for the Department
        public int DepartmentID { get; set; } // <--- FIXES CS0117 for DepartmentID

        // Navigation Properties

        // Navigation property to the related Department entity.
        // This is what fixes the 'does not contain a definition for 'Department'' errors.
        public Department Department { get; set; } // <--- FIXES CS1061 for Department

        // Collection navigation property for enrollments (many-to-many relationship with Student)
        public ICollection<Enrollment> Enrollments { get; set; }

        // Collection navigation property for course assignments (many-to-many relationship with Instructor)
        public ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}
