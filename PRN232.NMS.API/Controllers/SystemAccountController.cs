using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRN232.NMS.API.Models.RequestModels;
using PRN232.NMS.API.Models.RequestModels.SystemAccountRequests;
using PRN232.NMS.API.Models.ResponseModels;
using PRN232.NMS.API.Models.ResponseModels.SystemAccountResponses;
using PRN232.NMS.Services.Interfaces;

namespace PRN232.NMS.API.Controllers
{
    [ApiController]
    public class SystemAccountController : ControllerBase
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IMapper _mapper;

        public SystemAccountController(ISystemAccountService systemAccountService, IMapper mapper)
        {
            _systemAccountService = systemAccountService;
            _mapper = mapper;
        }

        [HttpPost("api/authentication")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var userAccount = await _systemAccountService.GetUserAccount(loginRequest.Username, loginRequest.Password);

             var mappedUser = _mapper.Map<LoginResponse>(userAccount);


            if (mappedUser != null)
            {
                var response = new ResponseDTO<LoginResponse>(message:"Login successfully", isSuccess:true, data:mappedUser, errors:null);
                
                return Ok(response);
            }

            return Unauthorized(new ResponseDTO<LoginResponse>(message:"Failed", isSuccess:true, data:null, errors:null));
        }

        [HttpGet("api/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _systemAccountService.GetAllUser();
            return Ok(users);
        }
    }
}
