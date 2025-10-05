using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public class Instructor
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstMidName { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        // Navigation properties
        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
        public OfficeAssignment? OfficeAssignment { get; set; }

        // Helper property
        public string FullName => $"{FirstMidName} {LastName}";
    }
}
