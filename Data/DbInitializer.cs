using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Xml.Linq;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            if (context.Students.Any())
            {
                return;
            }
        }

        public static void InitializeFromXml(SchoolContext context, String xmlFile)
        {
            if (context.Students.Any())
            {
                return;
            }

            XDocument doc = XDocument.Load(xmlFile);

            var students = doc.Descendants("student").Select(s => new Student
            {
                ID = int.Parse(s.Element("ID").Value),
                LastName = s.Element("LastName").Value,
                FirstMidName = s.Element("FirstMidName").Value,
                EnrollmentDate = DateTime.Parse(s.Element("EnrollmentDate").Value)
            }).ToArray();

            var instructors = doc.Descendants("instructor").Select(i => new Instructor
            {
                ID = int.Parse(i.Element("ID").Value),
                LastName = i.Element("LastName").Value,
                FirstMidName = i.Element("FirstMidName").Value,
                HireDate = DateTime.Parse(i.Element("HireDate").Value)
            }).ToArray();

            var departments = doc.Descendants("department").Select(d => new Department
            {
                DepartmentID = int.Parse(d.Element("DepartmentID").Value),
                Name = d.Element("Name").Value,
                Budget = decimal.Parse(d.Element("Budget").Value),
                StartDate = DateTime.Parse(d.Element("StartDate").Value),
                InstructorID = int.Parse(d.Element("InstructorID").Value)
            }).ToArray();

            var courses = doc.Descendants("course").Select(c => new Course
            {
                CourseID = int.Parse(c.Element("CourseID").Value),
                Title = c.Element("Title").Value,
                Credits = int.Parse(c.Element("Credits").Value),
                DepartmentID = int.Parse(c.Element("DepartmentID").Value)
            }).ToArray();

            var officeAssignments = doc.Descendants("officeAssignment").Select(oa => new OfficeAssignment
            {
                InstructorID = int.Parse(oa.Element("InstructorID").Value),
                Location = oa.Element("Location").Value
            }).ToArray();

            var courseAssignments = doc.Descendants("courseAssignment").Select(ca => new CourseAssignment
            {
                CourseID = int.Parse(ca.Element("CourseID").Value),
                InstructorID = int.Parse(ca.Element("InstructorID").Value)
            }).ToArray();

            var enrollments = doc.Descendants("enrollment").Select(e => new Enrollment
            {
                StudentID = int.Parse(e.Element("StudentID").Value),
                CourseID = int.Parse(e.Element("CourseID").Value),
                Grade = e.Element("Grade") != null ? Enum.Parse<Grade>(e.Element("Grade").Value) : (Grade?)null
            }).ToArray();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Students.AddRange(students);
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Student ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Student OFF;");

                    context.Instructors.AddRange(instructors);
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Instructor ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Instructor OFF;");

                    context.Departments.AddRange(departments);
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Department ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Department OFF;");

                    context.Courses.AddRange(courses);
                    context.OfficeAssignments.AddRange(officeAssignments);
                    context.CourseAssignments.AddRange(courseAssignments);
                    context.Enrollments.AddRange(enrollments);

                    context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}

