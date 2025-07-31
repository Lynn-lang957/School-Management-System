using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
        public async Task<IActionResult> GetMyProfile()
        {
            // Try to get the UserId from the JWT claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            // Find the teacher using the linked UserId
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (teacher == null)
                return NotFound("Teacher profile not found.");

            // Return teacher data as DTO
            var teacherDto = new TeacherDTO
            {
                Id = teacher.Id,
                FullName = teacher.FullName,
                Email = teacher.Email,
                Subject = teacher.Subject
            };

            return Ok(teacherDto);
        }
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
        public async Task<IActionResult> EnrollStudentToMyCourse([FromBody] EnrollmentDto dto)
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (string.IsNullOrEmpty(userId))
        return Unauthorized("User ID not found in token.");

    var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.UserId == userId);
    if (teacher == null)
        return NotFound("Teacher not found.");

    var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == dto.CourseId && c.TeacherId == teacher.Id);
    if (course == null)
        return BadRequest("Course not found or not owned by you.");

    var student = await _context.Students.FindAsync(dto.StudentId);
    if (student == null)
        return NotFound("Student not found.");

    // Optional: Check if already enrolled
    var alreadyEnrolled = await _context.Enrollments
        .AnyAsync(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId);
    if (alreadyEnrolled)
        return Conflict("Student is already enrolled in this course.");

    var enrollment = new Enrollment
    {
        StudentId = dto.StudentId,
        CourseId = dto.CourseId
    };

    _context.Enrollments.Add(enrollment);
    await _context.SaveChangesAsync();

    return Ok(new
    {
        Message = "Student successfully enrolled.",
        StudentId = dto.StudentId,
        CourseId = dto.CourseId
    });
}
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
