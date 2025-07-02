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

        // Later: we'll add TeacherId, Enrollments, etc.
    }
}
