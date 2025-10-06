using ContosoUniversity.Data;
using ContosoUniversity.Models;
using System;
using System.Linq;

namespace ContosoUniversity.Tests
{
    public static class Utilities
    {
        public static void InitializeDbForTests(SchoolContext db)
        {
            if (db.Students.Any()) return;

            var students = new[]
            {
                new Student { FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2010-09-01") },
                new Student { FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2013-09-01") }
            };
            db.Students.AddRange(students);

            var courses = new[]
            {
                new Course { CourseID = 1050, Title = "Chemistry", Credits = 3 },
                new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3 },
                new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3 }
            };
            db.Courses.AddRange(courses);

            var enrollments = new[]
            {
                new Enrollment { Student = students[0], Course = courses[0], Grade = Grade.A },
                new Enrollment { Student = students[1], Course = courses[1], Grade = Grade.C },
                new Enrollment { Student = students[2], Course = courses[2], Grade = Grade.B }
            };
            db.Enrollments.AddRange(enrollments);

            db.SaveChanges();
        }
    }
}