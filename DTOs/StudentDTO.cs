using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
    }
}
