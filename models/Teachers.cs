using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public string Subject { get; set; } = string.Empty;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
