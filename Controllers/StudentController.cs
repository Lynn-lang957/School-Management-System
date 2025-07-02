using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.DTOs;


namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Student
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // POST: api/Student
        [HttpPost]
public async Task<ActionResult<Student>> CreateStudent(StudentDTO dto)
{
    var student = new Student
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        DateOfBirth = dto.DateOfBirth
    };

    _context.Students.Add(student);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetStudents), new { id = student.Id }, student);
}

        // PUT: api/Student/5
[HttpPut("{id}")]
public async Task<IActionResult> UpdateStudent(int id, StudentDTO dto)
{
    
    var student = await _context.Students.FindAsync(id);
    if (student == null)
    {
        return NotFound();
    }

    student.FirstName = dto.FirstName;
    student.LastName = dto.LastName;
    student.Email = dto.Email;
    student.DateOfBirth = dto.DateOfBirth;

    await _context.SaveChangesAsync();

    return NoContent(); // 204 - success but no return body
}
// DELETE: api/Student/5
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteStudent(int id)
{
    var student = await _context.Students.FindAsync(id);
    if (student == null)
    {
        return NotFound();
    }

    _context.Students.Remove(student);
    await _context.SaveChangesAsync();

    return NoContent();
}

    }
}
