using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using StudentManagementSystem.DTOs;
using StudentManagementSystem.Services;
using System.Threading.Tasks;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userRequest)
        {
            var (success, message, registeredUser) = await _userService.RegisterUserAsync(userRequest);

            if (!success)
            {
                return BadRequest(new
                {
                    message,
                    success
                });
            }

            return Ok(new
            {
                message,
                success,
                user = new
                {
                    registeredUser!.Id,
                    registeredUser.Name,
                    registeredUser.Email,
                    registeredUser.Role
                }
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginDto)
        {
            var (success, message, user) = await _userService.LoginUserAsync(loginDto);

            if (!success)
            {
                return Unauthorized(new
                {
                    message,
                    success,
                    user
                });
            }

            return Ok(new
            {
                message,
                success,
                user
            });
        }


        [HttpGet("getAllUsersByRole")]
        public async Task<IActionResult> GetAllUsersByRole(string role)
        {
            var users = await _userService.GetUsersByRoleAsync(role);

            if (users == null || !users.Any())
            {
                return NotFound("No " + role + " found.");
            }

            var userList = users.Select(t => new RoleDto
            {
                Id = t.Id,
                Name = t.Name,
                Role = t.Role
            });
            return Ok(userList);
        }
    }
}
