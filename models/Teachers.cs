using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;
    }
}
