using ContosoUniversity.Models;
using System;
using System.Linq;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            if (context.Students.Any())
                return; // Already seeded

            SeedData(context);
        }

        public static void SeedInMemory(SchoolContext context)
        {
            // Ensure we start fresh each test run
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            SeedData(context);
        }

        private static void SeedData(SchoolContext context)
        {
            var students = new Student[]
            {
                new Student { FirstMidName = "Carson",   LastName = "Alexander", EnrollmentDate = DateTime.Parse("2016-09-01") },
                new Student { FirstMidName = "Meredith", LastName = "Alonso",    EnrollmentDate = DateTime.Parse("2018-09-01") },
                new Student { FirstMidName = "Arturo",   LastName = "Anand",     EnrollmentDate = DateTime.Parse("2019-09-01") },
            };

            context.Students.AddRange(students);
            context.SaveChanges();

            // Add additional seed data (courses, enrollments, etc.) as needed.
        }
    }
}
