using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ContosoUniversity.Tests
{
    public class DbExportServiceTests
    {
        private readonly DbContextOptions<SchoolContext> _options;
        public DbExportServiceTests() => _options = new DbContextOptionsBuilder<SchoolContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        [Fact]
        public void ExportToXml_WithData_CreatesCorrectRootElement()
        {
            using var context = new SchoolContext(_options);
            context.Students.Add(new Student { FirstMidName = "Test", LastName = "Student", EnrollmentDate = DateTime.Now });
            context.SaveChanges();
            var service = new DbExportService(context);
            XDocument result = service.ExportToXml();
            Assert.NotNull(result);
            Assert.Equal("School", result.Root.Name.LocalName);
        }
    }
}
