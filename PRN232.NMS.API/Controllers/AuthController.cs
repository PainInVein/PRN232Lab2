using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.NMS.Services.Interfaces;
using PRN232.NMS.Services.Models;
using PRN232.NMS.Services.Models.RequestModels.Auth;
using PRN232.NMS.Services.Models.ResponseModels;

namespace PRN232.NMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null)
            {
                return Unauthorized(new ResponseDTO<string>("Login Fail", false, null, "Invalid email or password"));
            }
            return Ok(new ResponseDTO<string>("Login successful", true, response, null));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel request)
        {
            var response = await _authService.RegisterAsync(request);
            if (response == null)
            {
                return BadRequest(new ResponseDTO<string>("Registration failed", false, null, "User already exist" ));
            }
            return Ok(new ResponseDTO<object>("Registration successful", true, response, null));
        }
    }
}
