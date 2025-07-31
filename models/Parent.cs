namespace SchoolAPI.Models
{
    public class Parent
    {
        public int Id { get; set; }
        public string FullName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string? UserId { get; set; }
        
        public ApplicationUser User { get; set; } = null!;
        public List<Student> Students { get; set; } = new List<Student>();
    }
}
