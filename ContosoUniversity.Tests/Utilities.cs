using System;
using System.Linq;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Tests;

namespace ContosoUniversity.Tests
{
    public static class Utilities
    {
        public static void InitializeDbForTests(SchoolContext db)
        {
            if (db.Students.Any())
                return;

            var students = new Student[]
            {
                new Student { ID = 1, FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2019-09-01") },
                new Student { ID = 2, FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2017-09-01") },
                new Student { ID = 3, FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2018-09-01") },
                new Student { ID = 4, FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2017-09-01") }
            };

            db.Students.AddRange(students);
            db.SaveChanges();
        }

        public static Student GetStudent(SchoolContext db, int id)
        {
            return db.Students.SingleOrDefault(s => s.ID == id) ?? throw new InvalidOperationException($"Student with ID {id} not found.");
        }

        public static IQueryable<Student> GetAllStudents(SchoolContext db)
        {
            return db.Students.OrderBy(s => s.LastName);
        }
    }
}
