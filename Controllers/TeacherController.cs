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

        // GET: api/Teacher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return await _context.Teachers.ToListAsync();
        }

        // POST: api/Teacher
        [HttpPost]
        public async Task<ActionResult<Teacher>> CreateTeacher(TeacherDTO dto)
        {
            var teacher = new Teacher
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        Subject = dto.Subject
    };
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeachers), new { id = teacher.Id }, teacher);
        }
        // PUT: api/Teacher/1
[HttpPut("{id}")]
public async Task<IActionResult> UpdateTeacher(int id, TeacherDTO dto)
{
    if (id != dto.Id)
    {
        return BadRequest("Teacher ID mismatch.");
    }

    var teacher = await _context.Teachers.FindAsync(id);
    if (teacher == null)
    {
        return NotFound();
    }

    // Update fields
    teacher.FirstName = dto.FirstName;
    teacher.LastName = dto.LastName;
    teacher.Email = dto.Email;
    teacher.Subject = dto.Subject;

    await _context.SaveChangesAsync();

    return NoContent(); // 204
}
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
