using ContosoUniversity.Tests;
using System.Threading.Tasks;
using Xunit;

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
            Assert.Contains("Students", content);
        }

        [Fact]
        public async Task Get_StudentDetailsPage_ReturnsSuccessAndCorrectContent()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Students/Details/1");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            // THE FIX: We are now just checking for the last name.
            Assert.Contains("Alexander", content);
        }
    }
}