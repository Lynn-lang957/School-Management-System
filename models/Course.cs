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
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; } = null!;
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

        // Later: we'll add TeacherId, Enrollments, etc.
    }
}
