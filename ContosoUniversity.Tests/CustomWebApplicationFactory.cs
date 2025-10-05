using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ContosoUniversity.Tests
{
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
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
                    options.UseInMemoryDatabase("TestDb"));

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<SchoolContext>();

                    db.Database.EnsureCreated();

                    // ✅ Seed test data
                    if (!db.Students.Any())
                    {
                        db.Students.Add(new Student
                        {
                            ID = 1,
                            FirstMidName = "Test",
                            LastName = "Student",
                            EnrollmentDate = DateTime.UtcNow
                        });
                        db.SaveChanges();
                    }
                }
            });
        }
    }
}
