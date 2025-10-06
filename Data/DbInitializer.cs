using System;
using System.Linq;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            if (context.Students.Any())
                return;

            var students = new Student[]
            {
                new Student { FirstMidName = "Alexander", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2019-09-01") },
                new Student { FirstMidName = "Alonso", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2019-09-01") },
                new Student { FirstMidName = "Anand", LastName = "Anand", EnrollmentDate = DateTime.Parse("2019-09-01") },
                new Student { FirstMidName = "Barzdukas", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2019-09-01") }
            };
            context.Students.AddRange(students);
            context.SaveChanges();

            var courses = new Course[]
            {
                new Course { CourseID = 1050, Title = "Chemistry", Credits = 3 },
                new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3 },
                new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3 },
                new Course { CourseID = 1045, Title = "Calculus", Credits = 4 }
            };
            context.Courses.AddRange(courses);
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment { StudentID = students[0].ID, CourseID = courses[0].CourseID, Grade = Grade.A },
                new Enrollment { StudentID = students[1].ID, CourseID = courses[1].CourseID, Grade = Grade.C },
                new Enrollment { StudentID = students[2].ID, CourseID = courses[2].CourseID, Grade = Grade.B },
                new Enrollment { StudentID = students[3].ID, CourseID = courses[3].CourseID, Grade = Grade.B }
            };
            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
        }
    }
}
