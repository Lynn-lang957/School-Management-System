using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.DTOs;

namespace SchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GradesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        public async Task<IActionResult> CreateGrade([FromBody] GradeDTO dto)
        {
            var grade = new Grade
            {
                StudentId = dto.StudentId,
                CourseId = dto.CourseId,
                Score = dto.Score,
                Remarks = dto.Remarks
            };

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return Ok(grade);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGrades()
        {
            var grades = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Course)
                .ToListAsync();
            return Ok(grades);
        }

        [Authorize(Roles = "Admin, Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrade(int id, [FromBody] GradeDTO dto)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return NotFound();

            grade.Score = dto.Score;
            grade.Remarks = dto.Remarks;

            await _context.SaveChangesAsync();
            return Ok(grade);
        }

       [Authorize(Roles = "Admin, Teacher")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return NotFound();

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
