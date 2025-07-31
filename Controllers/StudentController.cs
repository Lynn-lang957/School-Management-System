using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.DTOs;
using System.Security.Claims;


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
        [Authorize(Roles = "Teacher")]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetStudentsInMyCourses()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var teacher = await _context.Teachers
        .Include(t => t.Courses)
            .ThenInclude(c => c.StudentCourses)
        .FirstOrDefaultAsync(t => t.UserId == userId);

    if (teacher == null)
        return NotFound("Teacher not found.");

    var students = teacher.Courses
        .SelectMany(c => c.StudentCourses)
        .Distinct()
        .ToList();

    return Ok(students);
}

        [Authorize(Roles = "Student")]
        [HttpGet("me")]
        public async Task<IActionResult> GetOwnProfile()
{
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var student = await _context.Students
        .FirstOrDefaultAsync(s => s.UserId == userId);

    if (student == null)
        return NotFound("Student profile not found.");

    return Ok(student);
}


        [Authorize(Roles = "Admin")]
        // POST: api/Student

        [HttpPost]
public async Task<ActionResult<Student>> CreateStudent(StudentDTO dto)
{
    var student = new Student
    {
        FullName = dto.FullName,
        Email = dto.Email,
        DateOfBirth = dto.DateOfBirth
    };

    _context.Students.Add(student);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetStudents), new { id = student.Id }, student);
}

       [Authorize(Roles = "Admin")]
        // PUT: api/Student/5
        [HttpPut("{id}")]
public async Task<IActionResult> UpdateStudent(int id, StudentDTO dto)
{
    
    var student = await _context.Students.FindAsync(id);
    if (student == null)
    {
        return NotFound();
    }

    student.FullName= dto.FullName;
    student.Email = dto.Email;
    student.DateOfBirth = dto.DateOfBirth;

    await _context.SaveChangesAsync();

    return NoContent(); // 204 - success but no return body
}
        [Authorize(Roles = "Admin")]
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
