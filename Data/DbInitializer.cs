using ContosoUniversity.Models;
using ContosoUniversity.Data;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        // Seed SQL Server DB
        public static void Initialize(SchoolContext context)
        {
            if (context.Students.Any()) return;

            var students = new Student[]
            {
                new Student { ID = 1, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2019-09-01") },
                new Student { ID = 2, FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2017-09-01") },
                new Student { ID = 3, FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2018-09-01") },
                new Student { ID = 4, FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2017-09-01") }
            };

            context.Students.AddRange(students);
            context.SaveChanges();
        }

        // Seed InMemory DB for tests
        public static void SeedInMemory(SchoolContext context)
        {
            context.Students.AddRange(new Student[]
            {
                new Student { ID = 1, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2019-09-01") },
                new Student { ID = 2, FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2017-09-01") },
                new Student { ID = 3, FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2018-09-01") },
                new Student { ID = 4, FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2017-09-01") }
            });

            context.SaveChanges();
        }
    }
}
