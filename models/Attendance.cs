namespace SchoolAPI.Models
{
    public enum AttendanceStatus
{
    Present,
    Absent,
    Late
}
public class Attendance
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public DateTime Date { get; set; }
        public required string Status { get; set; }
    }
}
