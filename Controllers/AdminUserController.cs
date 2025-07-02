using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTOs;
using SchoolAPI.Models;

namespace SchoolAPI.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]  // 💡 Only Admins can access
    public class AdminUserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // POST: api/admin/create-user
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var userExists = await _userManager.FindByNameAsync(dto.Username);
            if (userExists != null)
                return BadRequest("User already exists.");

            var user = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync(dto.Role))
                await _roleManager.CreateAsync(new IdentityRole(dto.Role));

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok($"User {dto.Username} created with role {dto.Role}.");
        }

        // GET: api/admin/list-users
        [HttpGet("list-users")]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.FullName
            });

            return Ok(users);
        }

        // DELETE: api/admin/delete-user/{id}
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            await _userManager.DeleteAsync(user);
            return Ok("User deleted successfully.");
        }
    }
}
