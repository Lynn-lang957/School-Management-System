using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;
using SchoolAPI.Models;
using SchoolAPI.DTOs;
[Route("api/[controller]")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AttendanceController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> MarkAttendance([FromBody] AttendanceDTO dto)
    {
        var attendance = new Attendance
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            Date = dto.Date,
            Status = dto.Status
        };

        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();
        return Ok(attendance);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var records = await _context.Attendances
            .Include(a => a.Student)
            .Include(a => a.Course)
            .ToListAsync();

        return Ok(records);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _context.Attendances.FindAsync(id);
        if (record == null) return NotFound();

        _context.Attendances.Remove(record);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
