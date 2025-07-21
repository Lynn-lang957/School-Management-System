using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
    }
}
