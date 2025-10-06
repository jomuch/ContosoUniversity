using ContosoUniversity;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContosoUniversity.Tests
{
    public class StudentPagesTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StudentPagesTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_StudentsIndexPage_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/Students");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Alice", content);
            Assert.Contains("Bob", content);
        }
    }
}
