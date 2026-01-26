using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.NMS.Services.Interfaces;
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
                return Unauthorized("Invalid credentials");
            }
            return Ok(new ResponseDTO<string>("Login successful", true, response, null));
        }
    }
}
