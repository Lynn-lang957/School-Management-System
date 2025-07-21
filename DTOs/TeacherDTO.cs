using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs
{
    public class TeacherDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be valid.")]
        public string Email { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;
    }
}
