using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models;

namespace SchoolAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>()
        .HasOne(a => a.Student)
        .WithOne()
        .HasForeignKey<ApplicationUser>(a => a.StudentId)
        .IsRequired(false); // optional

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Teacher)
                .WithOne()
                .HasForeignKey<ApplicationUser>(a => a.TeacherId)
                .IsRequired(false); // optional

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Parent)
                .WithOne()
                .HasForeignKey<ApplicationUser>(a => a.ParentId)
                .IsRequired(false); // optional

            // StudentCourse Many-to-Many
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);
            // STUDENT USER
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne()
                .HasForeignKey<Student>(s => s.UserId);
            // TEACHER USER
            modelBuilder.Entity<Teacher>()
              .HasOne(t => t.User)
              .WithOne(u => u.Teacher)
              .HasForeignKey<Teacher>(t => t.UserId);

            // PARENT USER
            modelBuilder.Entity<Parent>()
            .HasOne(p => p.User)
            .WithOne(u => u.Parent)
            .HasForeignKey<Parent>(p => p.UserId);
            modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);
            modelBuilder.Entity<Attendance>()
            .HasKey(e => new { e.StudentId, e.CourseId });
            modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Student)
            .WithMany(s => s.AttendanceRecords)
            .HasForeignKey(a => a.StudentId);
            modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Course)
            .WithMany(c => c.AttendanceRecords)
            .HasForeignKey(a => a.CourseId);
            modelBuilder.Entity<Parent>()
    .HasMany(p => p.Students)
    .WithOne(s => s.Parent)
    .HasForeignKey(s => s.ParentId)
    .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Grade>()
            .HasOne(g => g.Student)
            .WithMany(s => s.Grades)
            .HasForeignKey(g => g.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TeacherId)
             .IsRequired(false);  // ✅ Optional FK
            

        }
    }
}
