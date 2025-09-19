using ContosoUniversity.Models;
using System;
using System.Linq;
using ContosoUniversity.Data;
using System.Collections.Generic;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var students = new Student[]
            {
                new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2019-09-01")},
                new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2017-09-01")},
                new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2018-09-01")},
                new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2017-09-01")},
                new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2017-09-01")},
                new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2016-09-01")},
                new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2018-09-01")},
                new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2019-09-01")}
            };
            context.Students.AddRange(students);

            var instructors = new Instructor[]
            {
                new Instructor { FirstMidName = "Kim",     LastName = "Abercrombie", HireDate = DateTime.Parse("1995-03-11") },
                new Instructor { FirstMidName = "Fadi",    LastName = "Fakhouri",    HireDate = DateTime.Parse("2002-07-06") },
                new Instructor { FirstMidName = "Roger",   LastName = "Harui",       HireDate = DateTime.Parse("1998-07-01") },
                new Instructor { FirstMidName = "Candace", LastName = "Kapoor",      HireDate = DateTime.Parse("2001-01-15") },
                new Instructor { FirstMidName = "Roger",   LastName = "Zheng",       HireDate = DateTime.Parse("2004-02-12") }
            };
            context.Instructors.AddRange(instructors);

            var departments = new Department[]
            {
                new Department { Name = "English",     Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), Instructor = instructors.Single( i => i.LastName == "Abercrombie") },
                new Department { Name = "Mathematics", Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), Instructor = instructors.Single( i => i.LastName == "Fakhouri") },
                new Department { Name = "Engineering", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), Instructor = instructors.Single( i => i.LastName == "Harui") },
                new Department { Name = "Economics",   Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), Instructor = instructors.Single( i => i.LastName == "Kapoor") }
            };
            context.Departments.AddRange(departments);

            var courses = new Course[]
            {
                new Course {CourseID = 1050, Title = "Chemistry",      Credits = 3, Department = departments.Single( s => s.Name == "Engineering") },
                new Course {CourseID = 4022, Title = "Microeconomics", Credits = 3, Department = departments.Single( s => s.Name == "Economics") },
                new Course {CourseID = 4041, Title = "Macroeconomics", Credits = 3, Department = departments.Single( s => s.Name == "Economics") },
                new Course {CourseID = 1045, Title = "Calculus",       Credits = 4, Department = departments.Single( s => s.Name == "Mathematics") },
                new Course {CourseID = 3141, Title = "Trigonometry",   Credits = 4, Department = departments.Single( s => s.Name == "Mathematics") },
                new Course {CourseID = 2021, Title = "Composition",    Credits = 3, Department = departments.Single( s => s.Name == "English") },
                new Course {CourseID = 2042, Title = "Literature",     Credits = 4, Department = departments.Single( s => s.Name == "English") },
            };
            context.Courses.AddRange(courses);

            var officeAssignments = new OfficeAssignment[]
            {
                new OfficeAssignment { Instructor = instructors.Single( i => i.LastName == "Fakhouri"), Location = "Gowan 27" },
                new OfficeAssignment { Instructor = instructors.Single( i => i.LastName == "Harui"), Location = "Thompson 304" },
            };
            context.OfficeAssignments.AddRange(officeAssignments);

            var courseAssignments = new CourseAssignment[]
            {
                new CourseAssignment { Course = courses.Single(c => c.Title == "Chemistry" ), Instructor = instructors.Single(i => i.LastName == "Abrcrombie") },
                new CourseAssignment { Course = courses.Single(c => c.Title == "Microeconomics" ), Instructor = instructors.Single(i => i.LastName == "Fakhouri") },
                new CourseAssignment { Course = courses.Single(c => c.Title == "Macroeconomics" ), Instructor = instructors.Single(i => i.LastName == "Harui") },
                new CourseAssignment { Course = courses.Single(c => c.Title == "Calculus" ), Instructor = instructors.Single(i => i.LastName == "Harui") },
                new CourseAssignment { Course = courses.Single(c => c.Title == "Trigonometry" ), Instructor = instructors.Single(i => i.LastName == "Harui") },
                new CourseAssignment { Course = courses.Single(c => c.Title == "Composition" ), Instructor = instructors.Single(i => i.LastName == "Harui") },
                new CourseAssignment { Course = courses.Single(c => c.Title == "Literature" ), Instructor = instructors.Single(i => i.LastName == "Harui") },
            };
            context.CourseAssignments.AddRange(courseAssignments);

            var enrollments = new Enrollment[]
            {
                new Enrollment { Student = students.Single(s => s.LastName == "Alexander" ), Course = courses.Single(c => c.Title == "Chemistry" ), Grade = Grade.A },
                new Enrollment { Student = students.Single(s => s.LastName == "Alexander" ), Course = courses.Single(c => c.Title == "Microeconomics" ), Grade = Grade.C },
                new Enrollment { Student = students.Single(s => s.LastName == "Alexander" ), Course = courses.Single(c => c.Title == "Macroeconomics" ), Grade = Grade.B },
                new Enrollment { Student = students.Single(s => s.LastName == "Alonso" ), Course = courses.Single(c => c.Title == "Calculus" ), Grade = Grade.B },
                new Enrollment { Student = students.Single(s => s.LastName == "Alonso" ), Course = courses.Single(c => c.Title == "Trigonometry" ), Grade = Grade.F },
                new Enrollment { Student = students.Single(s => s.LastName == "Alonso" ), Course = courses.Single(c => c.Title == "Composition" ), Grade = Grade.F },
                new Enrollment { Student = students.Single(s => s.LastName == "Anand" ), Course = courses.Single(c => c.Title == "Chemistry" )},
                new Enrollment { Student = students.Single(s => s.LastName == "Barzdukas" ), Course = courses.Single(c => c.Title == "Chemistry" )},
                new Enrollment { Student = students.Single(s => s.LastName == "Barzdukas" ), Course = courses.Single(c => c.Title == "Microeconomics" ), Grade = Grade.F },
                new Enrollment { Student = students.Single(s => s.LastName == "Li" ), Course = courses.Single(c => c.Title == "Macroeconomics" ), Grade = Grade.C },
                new Enrollment { Student = students.Single(s => s.LastName == "Justice" ), Course = courses.Single(c => c.Title == "Calculus" )},
                new Enrollment { Student = students.Single(s => s.LastName == "Norman" ), Course = courses.Single(c => c.Title == "Trigonometry" ), Grade = Grade.A },
            };
            context.Enrollments.AddRange(enrollments);

            context.SaveChanges();
        }
    }
}