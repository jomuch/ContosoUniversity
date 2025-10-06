using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ContosoUniversity.Tests
{
    public class StudentPagesTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public StudentPagesTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (!context.Students.Any())
            {
                context.Students.Add(new Student
                {
                    FirstMidName = "Test",
                    LastName = "Student",
                    EnrollmentDate = System.DateTime.Now
                });
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task Get_StudentDetailsPage_ReturnsSuccessAndCorrectContent()
        {
            using var scope = new CustomWebApplicationFactory().Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
            var student = await context.Students.FirstAsync();

            var response = await _client.GetAsync($"/Students/Details/{student.ID}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(student.LastName, content);
        }
    }
}
