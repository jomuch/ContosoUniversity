using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ContosoUniversity.Tests
{
    public class StudentPagesTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public StudentPagesTests(CustomWebApplicationFactory<Program> factory) => _factory = factory;

        [Fact]
        public async Task Get_StudentsIndexPage_ReturnsSuccessAndCorrectContentType()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Students");

            response.EnsureSuccessStatusCode(); // Status 200-299
            Assert.Equal("text/html; charset=utf-8",
                         response.Content.Headers.ContentType.ToString());
        }
    }
}
