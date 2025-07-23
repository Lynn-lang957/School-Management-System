using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data; 
using SchoolAPI.Models; 

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EnrollmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin")]
    // POST: api/Enrollment
    [HttpPost]
    public async Task<IActionResult> EnrollStudent([FromBody] EnrollmentDto dto)
    {
        var student = await _context.Students.FindAsync(dto.StudentId);
        var course = await _context.Courses.FindAsync(dto.CourseId);

        if (student == null || course == null)
        {
            return NotFound("Student or Course not found.");
        }

        var alreadyExists = await _context.Enrollments.AnyAsync(e => 
            e.StudentId == dto.StudentId && e.CourseId == dto.CourseId);

        if (alreadyExists)
        {
            return BadRequest("Student already enrolled in this course.");
        }

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return Ok("Student enrolled successfully.");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllEnrollments()
    {
        var enrollments = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .ToListAsync();

        return Ok(enrollments);
    }


    [HttpGet("by-student/{studentId}")]
    public async Task<IActionResult> GetEnrollmentsByStudent(int studentId)
    {
        var enrollments = await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == studentId)
            .ToListAsync();

        return Ok(enrollments);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEnrollment(int studentId, int courseId)
    {
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        if (enrollment == null)
        {
            return NotFound("Enrollment not found.");
        }

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return Ok("Enrollment deleted.");
    }
}
