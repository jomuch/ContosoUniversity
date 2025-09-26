using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public static class DbExporter
    {
        public static void ExportToXml(SchoolContext context, string outputFile)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (string.IsNullOrWhiteSpace(outputFile))
                throw new ArgumentNullException(nameof(outputFile));

            // Define XName constants outside the query
            XName studentsTag = "Students";
            XName studentTag = "Student";
            XName firstNameTag = "FirstName";
            XName lastNameTag = "LastName";
            XName enrollmentDateTag = "EnrollmentDate";

            // Load data from database first to avoid client-side constant issues
            var studentList = context.Students
                .Select(s => new
                {
                    s.FirstMidName,   // <-- use FirstMidName instead of FirstName
                    s.LastName,
                    s.EnrollmentDate
                })
                .ToList(); // Materialize the query

            // Create XML
            var xml = new XElement(studentsTag,
                studentList.Select(s =>
                    new XElement(studentTag,
                        new XElement(firstNameTag, s.FirstMidName ?? ""), // handle nulls
                        new XElement(lastNameTag, s.LastName ?? ""),
                        new XElement(enrollmentDateTag, s.EnrollmentDate.ToString("yyyy-MM-dd"))
                    )
                )
            );

            // Save to file
            xml.Save(outputFile);
        }
    }
}
