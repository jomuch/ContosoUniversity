using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        // This is the Foreign Key to the Instructor table
        public int? InstructorID { get; set; }

        // This is the Navigation Property for the Administrator (Instructor)
        public Instructor? Administrator { get; set; }

        public ICollection<Course>? Courses { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}