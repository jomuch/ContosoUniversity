$filePath = ".\Services\DbExportService.cs"

# Ensure the Services directory exists
$dirPath = Split-Path -Parent $filePath
if (-not (Test-Path -Path $dirPath -PathType Container)) {
    New-Item -Path $dirPath -ItemType Directory | Out-Null
}

# Content of DbExportService.cs (Includes ICourseService implementation)
$fileContent = @"
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace ContosoUniversity.Services
{
    public class DbExportService : ICourseService
    {
        private readonly SchoolContext _context;

        public DbExportService(SchoolContext context)
        {
            _context = context;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }

        public Course? GetCourseById(int id)
        {
            return _context.Courses.FirstOrDefault(c => c.CourseID == id);
        }

        public XDocument ExportToXml()
        {
            var students = _context.Students.Select(s => new XElement("student",
                new XElement("ID", s.ID),
                new XElement("FirstMidName", s.FirstMidName),
                new XElement("LastName", s.LastName),
                new XElement("EnrollmentDate", s.EnrollmentDate.ToString("yyyy-MM-dd"))
            ));

            return new XDocument(
                new XElement("School",
                    new XElement("Students", students)
                )
            );
        }
    }
}
"@

# Write the content to the file
$fileContent | Out-File -FilePath $filePath -Encoding UTF8

Write-Host "Successfully created service implementation file at: $filePath"