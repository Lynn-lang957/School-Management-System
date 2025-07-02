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
