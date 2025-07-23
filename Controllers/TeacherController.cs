using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.DTOs;


namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }
[Authorize(Roles = "Admin")]
        // GET: api/Teacher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return await _context.Teachers.ToListAsync();
        }
[Authorize(Roles = "Teacher")]
        [HttpGet("me")]
        public IActionResult GetMyProfile() => Ok("Teacher profile");
        [Authorize(Roles = "Admin")]
        // POST: api/Teacher
        [HttpPost]
        public async Task<ActionResult<Teacher>> CreateTeacher(TeacherDTO dto)
        {
            var teacher = new Teacher
    {
        FullName = dto.FullName,         
        Email = dto.Email,
        Subject = dto.Subject
    };
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeachers), new { id = teacher.Id }, teacher);
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost("enroll-student")]
        public IActionResult EnrollStudentToMyCourse() => Ok("Student enrolled to teacher's course");
        [Authorize(Roles = "Admin")]
        // PUT: api/Teacher/1
        [HttpPut("{id}")]
public async Task<IActionResult> UpdateTeacher(int id, TeacherDTO dto)
{
    if (id != dto.Id)
    {
        return BadRequest("Teacher ID mismatch.");
    }

    Teacher? teacher = await _context.Teachers.FindAsync(id);
    if (teacher == null)
    {
        return NotFound();
    }

    // Update fields
    teacher.FullName = dto.FullName;
    teacher.Email = dto.Email;
    teacher.Subject = dto.Subject;

    await _context.SaveChangesAsync();

    return NoContent(); // 204
}
        [Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteTeacher(int id)
{
    var Teacher = await _context.Teachers.FindAsync(id);
    if (Teacher == null)
    {
        return NotFound();
    }

    _context.Teachers.Remove(Teacher);
    await _context.SaveChangesAsync();

    return NoContent();
}
    }
}
