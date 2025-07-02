using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10.")]
        public int Credits { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
