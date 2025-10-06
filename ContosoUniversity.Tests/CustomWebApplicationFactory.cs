using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace ContosoUniversity.Tests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove SQL Server DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<SchoolContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Add InMemory DbContext
                services.AddDbContext<SchoolContext>(options =>
                    options.UseInMemoryDatabase("InMemoryDbForTesting"));

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SchoolContext>();

                // Clear the database before seeding
                db.Students.RemoveRange(db.Students);
                db.Departments.RemoveRange(db.Departments);
                db.Courses.RemoveRange(db.Courses);
                db.Enrollments.RemoveRange(db.Enrollments);
                db.SaveChanges();

                SeedTestData(db);
            });
        }

        private void SeedTestData(SchoolContext context)
        {
            // Add students
            context.Students.AddRange(
                new Student { FirstMidName = "Alice", LastName = "Smith" },
                new Student { FirstMidName = "Bob", LastName = "Johnson" }
            );

            // Add departments
            context.Departments.AddRange(
                new Models.Department { DepartmentID = 1, Name = "Science" },
                new Models.Department { DepartmentID = 2, Name = "Math" }
            );

            // Add courses
            context.Courses.AddRange(
                new Course { CourseID = 1, Title = "Chemistry", Credits = 3, DepartmentID = 1 },
                new Course { CourseID = 2, Title = "Algebra", Credits = 4, DepartmentID = 2 }
            );

            context.SaveChanges();
        }
    }
}
