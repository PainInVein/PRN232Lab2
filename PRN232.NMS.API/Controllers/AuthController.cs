using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.NMS.Services.Models;
using PRN232.NMS.Services.Models.RequestModels.Auth;
using PRN232.NMS.Services.Models.ResponseModels;
using PRN232.NMS.Services.Models.ResponseModels.SystemAccountResponses;
using PRN232.NMS.Services.Interfaces;

namespace PRN232.NMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            var response = await _authService.LoginAsync(request.Email, request.Password);
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
            var user = await _authService.RegisterAsync(request.Email, request.Name, request.Password);

            var response = _mapper.Map<UserResponse>(user);

            if (response == null)
            {
                return BadRequest(new ResponseDTO<string>("Registration failed", false, null, "User already exist"));
            }
            return Ok(new ResponseDTO<object>("Registration successful", true, response, null));
        }
    }
}
