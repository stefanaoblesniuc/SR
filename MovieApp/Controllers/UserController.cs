using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.DTO.User;
using MovieApp.Services;

namespace MovieApp.Controllers
{
    //[Route("User")]
    [ApiController] // This attribute specifies for the framework to add functionality to the controller such as binding multipart/form-data.
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly RecombeeService _recombeeService;

        public UserController(UserService userService, RecombeeService recombeeService)
        {
            _userService = userService;
            _recombeeService = recombeeService;
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

                await _recombeeService.AddUserAsync(user.Id.ToString());
                return CreatedAtAction(nameof(Login), new { username = user.Username }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
