using System.Linq;
using System.Xml.Linq;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public static class DbExporter
    {
        public static void ExportToXml(SchoolContext context, string outputFile)
        {
            var studentData = context.Students.Select(s => new
            {
                s.ID,
                s.FirstMidName,
                s.LastName,
                s.EnrollmentDate
            }).ToList();

            var studentElements = studentData.Select(s =>
                new XElement("student",
                    new XElement("ID", s.ID),
                    new XElement("FirstMidName", s.FirstMidName),
                    new XElement("LastName", s.LastName),
                    new XElement("EnrollmentDate", s.EnrollmentDate.ToString("yyyy-MM-dd"))
                )
            );

            var instructorData = context.Instructors.Select(i => new
            {
                i.ID,
                i.FirstMidName,
                i.LastName,
                i.HireDate
            }).ToList();

            var instructorElements = instructorData.Select(i =>
                new XElement("instructor",
                    new XElement("ID", i.ID),
                    new XElement("FirstMidName", i.FirstMidName),
                    new XElement("LastName", i.LastName),
                    new XElement("HireDate", i.HireDate.ToString("yyyy-MM-dd"))
                )
            );

            var departmentData = context.Departments.Select(d => new
            {
                d.DepartmentID,
                d.Name,
                d.Budget,
                d.StartDate,
                d.InstructorID
            }).ToList();

            var departmentElements = departmentData.Select(d =>
                new XElement("department",
                    new XElement("DepartmentID", d.DepartmentID),
                    new XElement("Name", d.Name),
                    new XElement("Budget", d.Budget),
                    new XElement("StartDate", d.StartDate.ToString("yyyy-MM-dd")),
                    d.InstructorID.HasValue ? new XElement("InstructorID", d.InstructorID) : null
                )
            );

            var courseData = context.Courses.Select(c => new
            {
                c.CourseID,
                c.Title,
                c.Credits,
                c.DepartmentID
            }).ToList();

            var courseElements = courseData.Select(c =>
                new XElement("course",
                    new XElement("CourseID", c.CourseID),
                    new XElement("Title", c.Title),
                    new XElement("Credits", c.Credits),
                    new XElement("DepartmentID", c.DepartmentID)
                )
            );

            var courseAssignmentData = context.CourseAssignments.Select(ca => new
            {
                ca.CourseID,
                ca.InstructorID
            }).ToList();

            var courseAssignmentElements = courseAssignmentData.Select(ca =>
                new XElement("courseAssignment",
                    new XElement("CourseID", ca.CourseID),
                    new XElement("InstructorID", ca.InstructorID)
                )
            );

            var enrollmentData = context.Enrollments.Select(e => new
            {
                e.StudentID,
                e.CourseID,
                e.Grade
            }).ToList();

            var enrollmentElements = enrollmentData.Select(e =>
                new XElement("enrollment",
                    new XElement("StudentID", e.StudentID),
                    new XElement("CourseID", e.CourseID),
                    e.Grade.HasValue ? new XElement("Grade", e.Grade.ToString()) : null
                )
            );

            XDocument exportDoc = new XDocument(
                new XElement("School",
                    new XElement("students", studentElements),
                    new XElement("instructors", instructorElements),
                    new XElement("departments", departmentElements),
                    new XElement("courses", courseElements),
                    new XElement("courseAssignments", courseAssignmentElements),
                    new XElement("enrollments", enrollmentElements)
                )
            );

            exportDoc.Save(outputFile);
        }
    }
}