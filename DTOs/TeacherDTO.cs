using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs
{
    public class TeacherDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be valid.")]
        public string Email { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;
    }
}
