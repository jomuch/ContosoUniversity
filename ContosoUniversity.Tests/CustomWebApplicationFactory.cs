using ContosoUniversity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ContosoUniversity.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<SchoolContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<SchoolContext>(options =>
                    options.UseInMemoryDatabase("InMemoryDbForTesting"));
            });
        }
    }
}
