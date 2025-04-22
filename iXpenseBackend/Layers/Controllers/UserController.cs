using iXpenseBackend.Layers.Services;
using iXpenseBackend.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace iXpenseBackend.Layers.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public UserController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        //Register a user
        [Route("Register")]
        [HttpPost]
        public async Task <IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _userService.RegisterUserAsync(registerDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", result.Message });
            }
            else
            {
                return Ok(new { Status = "Created", result.Message, result.UserId });
            }
        }

        //Login a user
        [Route("Login")]
        [HttpPost]
        public async Task <IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginUserAsync(loginDto);
            if (!result.IsSuccess)
            {
                return Unauthorized(new { Status = "Error", result.Message });
            }
            return Ok(new { Status = "Success", result.Message, result.Token });
        }
    }
}
