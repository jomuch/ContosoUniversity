
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Instructor
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [Column("FirstName")]
        [StringLength(50)]
        public string FirstMidName { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        public string FullName => LastName + ", " + FirstMidName;

        public ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();
        public OfficeAssignment? OfficeAssignment { get; set; }
        public ICollection<Department>? Departments { get; set; }
    }
}
