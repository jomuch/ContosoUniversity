using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using ContosoUniversity.Data;

namespace ContosoUniversity.Tests
{
    // The factory class must be public for the testing framework to access it.
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the production database context registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<SchoolContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add SchoolContext using an in-memory database provider for testing
                services.AddDbContext<SchoolContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Build the service provider for seeding
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<SchoolContext>();

                    // Ensure the database is clean and seed it
                    context.Database.EnsureCreated();

                    // Add minimal seed data so the test has something to query
                    Utilities.InitializeDbForTests(context);
                }
            });
        }
    }
}