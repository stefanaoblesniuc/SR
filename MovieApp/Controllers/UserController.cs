using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.DTO.User;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO request)
        {
            var user = await _userService.GetUser(request.Username, request.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok(new { message = "Login successful", user });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO request)
        {
            try
            {
                var user = await _userService.AddUser(request.Username, request.Password);
                return CreatedAtAction(nameof(Login), new { username = user.Username }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
