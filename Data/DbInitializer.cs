using ContosoUniversity.Models;
using System;
using System.Linq;
using System.Xml.Linq;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            // Ensure the database is created before trying to seed it.
            context.Database.EnsureCreated();

            // Check if the Students table has any data. If it does, we assume
            // the DB has already been seeded and we skip this process.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            // Load the XML file from the Data directory.
            XDocument doc = XDocument.Load("Data/SeedData.xml");

            // Parse the <students> section and create Student objects.
            var students = doc.Descendants("student").Select(s => new Student
            {
                ID = int.Parse(s.Element("ID")!.Value),
                FirstMidName = s.Element("FirstMidName")!.Value,
                LastName = s.Element("LastName")!.Value,
                EnrollmentDate = DateTime.Parse(s.Element("EnrollmentDate")!.Value)
            }).ToArray();

            // Parse the <instructors> section.
            var instructors = doc.Descendants("instructor").Select(i => new Instructor
            {
                ID = int.Parse(i.Element("ID")!.Value),
                FirstMidName = i.Element("FirstMidName")!.Value,
                LastName = i.Element("LastName")!.Value,
                HireDate = DateTime.Parse(i.Element("HireDate")!.Value)
            }).ToArray();

            // ... continue parsing for all other entities ...

            var departments = doc.Descendants("department").Select(d => new Department
            {
                DepartmentID = int.Parse(d.Element("DepartmentID")!.Value),
                Name = d.Element("Name")!.Value,
                Budget = decimal.Parse(d.Element("Budget")!.Value),
                StartDate = DateTime.Parse(d.Element("StartDate")!.Value),
                InstructorID = int.Parse(d.Element("InstructorID")!.Value)
            }).ToArray();

            var courses = doc.Descendants("course").Select(c => new Course
            {
                CourseID = int.Parse(c.Element("CourseID")!.Value),
                Title = c.Element("Title")!.Value,
                Credits = int.Parse(c.Element("Credits")!.Value),
                DepartmentID = int.Parse(c.Element("DepartmentID")!.Value)
            }).ToArray();

            var officeAssignments = doc.Descendants("officeAssignment").Select(oa => new OfficeAssignment
            {
                InstructorID = int.Parse(oa.Element("InstructorID")!.Value),
                Location = oa.Element("Location")!.Value
            }).ToArray();

            var courseAssignments = doc.Descendants("courseAssignment").Select(ca => new CourseAssignment
            {
                CourseID = int.Parse(ca.Element("CourseID")!.Value),
                InstructorID = int.Parse(ca.Element("InstructorID")!.Value)
            }).ToArray();

            var enrollments = doc.Descendants("enrollment").Select(e => new Enrollment
            {
                StudentID = int.Parse(e.Element("StudentID")!.Value),
                CourseID = int.Parse(e.Element("CourseID")!.Value),
                Grade = Enum.TryParse<Grade>(e.Element("Grade")?.Value, out var grade) ? grade : null
            }).ToArray();

            // Add all the parsed data to the context.
            context.Students.AddRange(students);
            context.Instructors.AddRange(instructors);
            context.Departments.AddRange(departments);
            context.Courses.AddRange(courses);
            context.OfficeAssignments.AddRange(officeAssignments);
            context.CourseAssignments.AddRange(courseAssignments);
            context.Enrollments.AddRange(enrollments);

            // Save all changes to the database in a single transaction.
            context.SaveChanges();
        }
    }
}