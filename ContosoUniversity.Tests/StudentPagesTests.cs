using Microsoft.AspNetCore.Mvc.Testing;
using System.Net; 
using System.Threading.Tasks; 
using Xunit;

namespace ContosoUniversity.Tests {
    public class StudentPagesTests : IClassFixture<WebApplicationFactory<Program>> {
        private readonly WebApplicationFactory<Program> _factory;
        public StudentPagesTests(WebApplicationFactory<Program> factory) { _factory = factory; }
        [Fact]
        public async Task Get_StudentsIndexPage_ReturnsSuccessAndCorrectContentType() {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Students");
            response.EnsureSuccessStatusCode(); Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
        [Fact]
        public async Task Get_StudentsDetailsPage_ReturnsNotFoundForInvalidId() {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Students/Details?id=999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
