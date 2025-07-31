using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.DTOs;
using SchoolAPI.Models;


namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Parent
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateParent([FromBody] ParentDto dto)
        {
            var parent = new Parent
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserId = dto.UserId
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            dto.Id = parent.Id;
            return Ok(dto);
        }
        [HttpPost("{parentId}/assign-student/{studentId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignStudentToParent(int parentId, int studentId)
        {
            var parent = await _context.Parents.Include(p => p.Students).FirstOrDefaultAsync(p => p.Id == parentId);
            var student = await _context.Students.FindAsync(studentId);

            if (parent == null || student == null) return NotFound("Parent or Student not found");

            student.ParentId = parent.Id;
            await _context.SaveChangesAsync();

            return Ok("Student assigned to parent");
        }

        // PUT: api/Parent/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateParent(int id, [FromBody] ParentDto dto)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null) return NotFound();

            parent.FullName = dto.FullName;
            parent.Email = dto.Email;
            await _context.SaveChangesAsync();

            return Ok(dto);
        }

        // GET: api/Parent/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetParent(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null) return NotFound();

            var dto = new ParentDto
            {
                Id = parent.Id,
                FullName = parent.FullName,
                Email = parent.Email,
                UserId = parent.UserId
            };

            return Ok(dto);
        }

        // GET: api/Parent
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllParents()
        {
            var parents = await _context.Parents
                .Select(p => new ParentDto
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    Email = p.Email,
                    UserId = p.UserId
                }).ToListAsync();

            return Ok(parents);
        }
        [HttpGet("my-children")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetMyChildren()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var parent = await _context.Parents
                .Include(p => p.Students)
                    .ThenInclude(s => s.Grades)
                .Include(p => p.Students)
                    .ThenInclude(s => s.AttendanceRecords)
                .Include(p => p.Students)
                    .ThenInclude(s => s.StudentCourses)
                        .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (parent == null) return Unauthorized("Parent not found");

            var result = parent.Students.Select(s => new
            {
                s.Id,
                s.FullName,
                s.Email,
                Grades = s.Grades.Select(g => new { g.Id, g.Course, g.Score }),
                Attendance = s.AttendanceRecords.Select(a => new { a.Id, a.Date, a.Status }),
                Courses = s.StudentCourses.Select(sc => new { sc.CourseId, sc.Course.Title })
            });

            return Ok(result);
        }
    }
}
