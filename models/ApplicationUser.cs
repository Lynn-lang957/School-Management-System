using Microsoft.AspNetCore.Identity;
namespace SchoolAPI.Models
{


    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public Student Student { get; set; } = null!;
        public Teacher Teacher { get; set; } = null!;
        public Parent Parent { get; set; } = null!;
    }
}