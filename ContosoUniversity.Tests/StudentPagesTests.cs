using System.Net;
using System.Threading.Tasks;
using Xunit;
using ContosoUniversity.Models;
using Microsoft.Extensions.DependencyInjection;
using ContosoUniversity.Data;
using System.Linq;

namespace ContosoUniversity.Tests
{
    public class StudentPagesTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public StudentPagesTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_StudentsIndexPage_ReturnsSuccessAndCorrectContent()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/Students");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // Verify that seeded students appear on the page
            Assert.Contains("Carson Alexander", content);
            Assert.Contains("Meredith Alonso", content);
            Assert.Contains("Arturo Anand", content);
        }

        [Fact]
        public async Task Get_StudentDetailsPage_ReturnsSuccessAndCorrectContent()
        {
            var client = _factory.CreateClient();

            // Use a seeded student's ID
            var response = await client.GetAsync("/Students/Details/1");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // Verify student details appear
            Assert.Contains("Carson Alexander", content);
            Assert.Contains("2019-09-01", content); // Enrollment Date
        }
    }
}
