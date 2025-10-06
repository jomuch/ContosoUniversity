using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace ContosoUniversity.Tests
{
    public class DbExportServiceTests
    {
        private readonly SchoolContext _context;

        public DbExportServiceTests()
        {
            var options = new DbContextOptionsBuilder<SchoolContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new SchoolContext(options);

            // Seed data
            _context.Students.Add(new Student { FirstMidName = "Test", LastName = "Student", EnrollmentDate = System.DateTime.Now });
            _context.SaveChanges();
        }

        [Fact]
        public void Students_CanBeAdded()
        {
            var count = _context.Students.Count();
            Assert.Equal(1, count);

            _context.Students.Add(new Student { FirstMidName = "New", LastName = "Student", EnrollmentDate = System.DateTime.Now });
            _context.SaveChanges();

            Assert.Equal(2, _context.Students.Count());
        }
    }
}
