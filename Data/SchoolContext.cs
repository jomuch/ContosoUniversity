using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Enrollment> Enrollments { get; set; } = null!;
        public DbSet<Instructor> Instructors { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; } = null!;
        public DbSet<CourseAssignment> CourseAssignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary key for CourseAssignment (many-to-many)
            modelBuilder.Entity<CourseAssignment>()
                .HasKey(ca => new { ca.CourseID, ca.InstructorID });

            // Configure one-to-one relationship between Instructor and OfficeAssignment
            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.OfficeAssignment)
                .WithOne(o => o.Instructor)
                .HasForeignKey<OfficeAssignment>(o => o.InstructorID);

            // Optional: Configure string length for Course.Title
            modelBuilder.Entity<Course>()
                .Property(c => c.Title)
                .HasMaxLength(50);

            // Optional: seed data can go here if needed
        }
    }
}
