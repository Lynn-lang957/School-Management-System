using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.Models
{
    public class Student
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
        public int? ParentId { get; set; }
        public Parent Parent { get; set; } = null!;
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
        
    }
}