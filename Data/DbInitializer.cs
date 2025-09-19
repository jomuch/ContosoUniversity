using ContosoUniversity.Models;
using System;
using System.Linq;
using ContosoUniversity.Data;
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
            context.SaveChanges();
            var instructors = new Instructor[]
            {
                new Instructor { FirstMidName = "Kim",    LastName = "Abercrombie", HireDate = DateTime.Parse("1995-03-11") },
                new Instructor { FirstMidName = "Fadi",   LastName = "Fakhouri",    HireDate = DateTime.Parse("2002-07-06") },
                new Instructor { FirstMidName = "Roger",  LastName = "Harui",       HireDate = DateTime.Parse("1998-07-01") },
                new Instructor { FirstMidName = "Candace", LastName = "Kapoor",      HireDate = DateTime.Parse("2001-01-15") },
                new Instructor { FirstMidName = "Roger",  LastName = "Zheng",       HireDate = DateTime.Parse("2004-02-12") }
            };
            context.Instructors.AddRange(instructors);
            context.SaveChanges();
            var departments = new Department[]
            {
                new Department { Name = "English",    Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 1 },
                new Department { Name = "Mathematics", Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 2 },
                new Department { Name = "Engineering", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 3 },
                new Department { Name = "Economics",    Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 4 }
            };
            context.Departments.AddRange(departments);
            context.SaveChanges();
            var courses = new Course[]
            {
                new Course {CourseID = 1050, Title = "Chemistry",      Credits = 3, DepartmentID = 3 },
                new Course {CourseID = 4022, Title = "Microeconomics", Credits = 3, DepartmentID = 4 },
                new Course {CourseID = 4041, Title = "Macroeconomics", Credits = 3, DepartmentID = 4 },
                new Course {CourseID = 1045, Title = "Calculus",       Credits = 4, DepartmentID = 2 },
                new Course {CourseID = 3141, Title = "Trigonometry",   Credits = 4, DepartmentID = 2 },
                new Course {CourseID = 2021, Title = "Composition",    Credits = 3, DepartmentID = 1 },
                new Course {CourseID = 2042, Title = "Literature",     Credits = 4, DepartmentID = 1 },
            };
            context.Courses.AddRange(courses);
            context.SaveChanges();
            var officeAssignments = new OfficeAssignment[]
            {
                new OfficeAssignment { InstructorID = 1, Location = "Smith 17" },
                new OfficeAssignment { InstructorID = 2, Location = "Gowan 27" },
                new OfficeAssignment { InstructorID = 3, Location = "Thompson 304" },
            };
            context.OfficeAssignments.AddRange(officeAssignments);
            context.SaveChanges();
            var courseAssignments = new CourseAssignment[]
            {
                new CourseAssignment { CourseID = 1050, InstructorID = 1 },
                new CourseAssignment { CourseID = 4022, InstructorID = 2 },
                new CourseAssignment { CourseID = 4041, InstructorID = 3 },
                new CourseAssignment { CourseID = 1045, InstructorID = 4 },
                new CourseAssignment { CourseID = 3141, InstructorID = 4 },
                new CourseAssignment { CourseID = 2021, InstructorID = 5 },
                new CourseAssignment { CourseID = 2042, InstructorID = 5 },
            };
            context.CourseAssignments.AddRange(courseAssignments);
            context.SaveChanges();
            var enrollments = new Enrollment[]
            {
                new Enrollment { StudentID = 1, CourseID = 1050, Grade = Grade.A },
                new Enrollment { StudentID = 1, CourseID = 4022, Grade = Grade.C },
                new Enrollment { StudentID = 1, CourseID = 4041, Grade = Grade.B },
                new Enrollment { StudentID = 2, CourseID = 1045, Grade = Grade.B },
                new Enrollment { StudentID = 2, CourseID = 3141, Grade = Grade.F },
                new Enrollment { StudentID = 2, CourseID = 2021, Grade = Grade.F },
                new Enrollment { StudentID = 3, CourseID = 1050 },
                new Enrollment { StudentID = 4, CourseID = 1050 },
                new Enrollment { StudentID = 4, CourseID = 4022, Grade = Grade.F },
                new Enrollment { StudentID = 5, CourseID = 4041, Grade = Grade.C },
                new Enrollment { StudentID = 6, CourseID = 1045 },
                new Enrollment { StudentID = 7, CourseID = 3141, Grade = Grade.A },
            };
            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
        }
    }
}