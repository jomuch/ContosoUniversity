using ContosoUniversity.Data;
using ContosoUniversity.Models; 
using Microsoft.EntityFrameworkCore; 
using Moq; 
using System; 
using System.Linq; 
using System.Xml.Linq; 
using Xunit;

namespace ContosoUniversity.Tests {
    public class DbExportServiceTests {
        private readonly DbContextOptions<SchoolContext> _options;
        public DbExportServiceTests() { _options = new DbContextOptionsBuilder<SchoolContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options; }
        [Fact]
        public void ExportToXml_WithData_CreatesCorrectRootElement() {
            using (var context = new SchoolContext(_options)) {
                context.Students.Add(new Student { FirstMidName = "Test", LastName = "Student", EnrollmentDate = DateTime.Now });
                context.SaveChanges();
                var service = new DbExportService(context);
                XDocument result = service.ExportToXml();
                Assert.NotNull(result); Assert.Equal("School", result.Root.Name);
            }
        }
        [Fact]
        public void ExportToXml_WithStudentData_CreatesStudentElementWithCorrectName() {
            using (var context = new SchoolContext(_options)) {
                var testStudent = new Student { ID = 1, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2019-09-01") };
                context.Students.Add(testStudent); context.SaveChanges();
                var service = new DbExportService(context);
                XDocument result = service.ExportToXml();
                var studentElement = result.Root?.Element("Students")?.Elements("student").FirstOrDefault();
                Assert.NotNull(studentElement); Assert.Equal("Alexander", studentElement.Element("LastName")?.Value); Assert.Equal("Carson", studentElement.Element("FirstMidName")?.Value);
            }
        }
    }
}
