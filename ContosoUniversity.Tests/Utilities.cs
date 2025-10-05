using ContosoUniversity.Data;
using ContosoUniversity.Models;
using System;

namespace ContosoUniversity.Tests
{
    public static class Utilities
    {
        public static void InitializeDbForTests(SchoolContext context)
        {
            context.Departments.Add(new Department { DepartmentID = 1, Name = "English" });
            context.Departments.Add(new Department { DepartmentID = 2, Name = "Mathematics" });

            context.Courses.Add(new Course { CourseID = 1050, Title = "Chemistry", Credits = 3, DepartmentID = 1 });
            context.Courses.Add(new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3, DepartmentID = 2 });

            context.Students.Add(new Student { ID = 1, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2019-09-01") });
            context.Students.Add(new Student { ID = 2, FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2017-09-01") });

            context.Enrollments.Add(new Enrollment { StudentID = 1, CourseID = 1050, Grade = Grade.A });
            context.Enrollments.Add(new Enrollment { StudentID = 2, CourseID = 4022, Grade = Grade.C });

            context.SaveChanges();
        }
    }
}