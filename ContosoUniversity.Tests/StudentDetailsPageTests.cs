using ContosoUniversity;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContosoUniversity.Tests
{
    public class StudentDetailsPageTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StudentDetailsPageTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_StudentDetailsPage_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/Students/Details/1");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Alice", content);
        }
    }
}
