using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ContosoUniversity.Tests
{
    // 1. Inherit from CustomWebApplicationFactory instead of the default WebApplicationFactory
    public class StudentPagesTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        // Use the custom factory type
        private readonly CustomWebApplicationFactory<Program> _factory;

        public StudentPagesTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_StudentsIndexPage_ReturnsSuccessAndCorrectContentType()
        {
            // The custom factory ensures a seeded database is available when the client is created
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Students");

            // This now checks against a successfully initialized, data-aware application
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_StudentsDetailsPage_ReturnsNotFoundForInvalidId()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Students/Details?id=999");

            // Should correctly return 404 Not Found since the custom database is empty except for seed data
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}