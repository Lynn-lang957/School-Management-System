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
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Course
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }
 [Authorize(Roles = "Student,Parent")]
        [HttpGet("enrolled")]
        public async Task<IActionResult> GetEnrolledCourses()
{
    var userId = User?.FindFirst("uid")?.Value ?? User?.Identity?.Name;

    if (string.IsNullOrEmpty(userId))
        return Unauthorized("User ID not found in token.");

    if (User.IsInRole("Student"))
    {
        var student = await _context.Students
            .Include(s => s.StudentCourses)
            .ThenInclude(sc => sc.Course)
            .FirstOrDefaultAsync(s => s.UserId == userId);

        if (student == null)
            return NotFound("Student profile not found.");

        var enrolledCourses = student.StudentCourses.Select(sc => new
        {
            sc.Course.Id,
            sc.Course.Title,
            sc.Course.Description,
            sc.Course.Credits
        });

        return Ok(enrolledCourses);
    }

    if (User.IsInRole("Parent"))
    {
        var parent = await _context.Parents
            .Include(p => p.User)
            .Include(p => p.Students)
                .ThenInclude(s => s.StudentCourses)
                    .ThenInclude(sc => sc.Course)
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (parent == null)
            return NotFound("Parent profile not found.");

        // Return all courses for all their children
        var result = parent.Students.Select(child => new
        {
            Student = child.FullName,
            Courses = child.StudentCourses.Select(sc => new
            {
                sc.Course.Id,
                sc.Course.Title,
                sc.Course.Description,
                sc.Course.Credits
            })
        });

        return Ok(result);
    }

    return Forbid();
}
        
        [Authorize(Roles = "Admin")]
        // POST: api/Course
        [HttpPost]
        public async Task<ActionResult<Course>> CreateCourse(CourseDTO dto)
        {
            var course = new Course
            {
                Title = dto.Title,
                Credits = dto.Credits,
                Description = dto.Description
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourses), new { id = course.Id }, course);
        }
       [Authorize(Roles = "Admin")]
        [HttpPost("assign-teacher")]
        public async Task<IActionResult> AssignTeacherToCourse([FromBody] AssignTeacherDTO dto)
{
    var course = await _context.Courses.FindAsync(dto.CourseId);
    if (course == null)
        return NotFound($"Course with ID {dto.CourseId} not found.");

    var teacher = await _context.Teachers.FindAsync(dto.TeacherId);
    if (teacher == null)
        return NotFound($"Teacher with ID {dto.TeacherId} not found.");

    course.TeacherId = teacher.Id;
    await _context.SaveChangesAsync();

    return Ok(new
    {
        Message = "Teacher assigned to course successfully.",
        Course = new
        {
            course.Id,
            course.Title,
            course.TeacherId
        }
    });
}

        [Authorize(Roles = "Admin")]
        // PUT: api/Course/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, CourseDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            course.Title = dto.Title;
            course.Credits = dto.Credits;
            course.Description = dto.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        // DELETE: api/Course/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
