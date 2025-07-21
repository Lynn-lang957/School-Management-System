using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs
{
    public class AttendanceDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public required string Status { get; set; }
            }
}
