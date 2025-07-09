using System.ComponentModel.DataAnnotations;

namespace SchoolAPI.DTOs
{
    public class GradeDTO
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        [Range(0, 100)]
        public double Score { get; set; }

        public string Remarks { get; set; } = string.Empty;
    }
}
