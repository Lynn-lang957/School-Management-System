using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Range(1, 10)]
        public int Credits { get; set; }

        public string Description { get; set; } = string.Empty;
        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        public ICollection<Attendance> AttendanceRecords { get; set; } = new List<Attendance>();

    }
}
