using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ContosoUniversity.Tests
{
    public class StudentDetailsPageTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public StudentDetailsPageTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_StudentDetailsPage_ReturnsSuccess()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ContosoUniversity.Data.SchoolContext>();

            // Seed a student
            var student = new Student { FirstMidName = "Test", LastName = "Student", EnrollmentDate = System.DateTime.Now };
            context.Students.Add(student);
            context.SaveChanges();

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"/Students/Details?id={student.ID}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Test", content);
            Assert.Contains("Student", content);
        }
    }
}
