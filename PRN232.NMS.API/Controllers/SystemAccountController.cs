using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.NMS.API.Models.RequestModels.SystemAccountRequests;
using PRN232.NMS.API.Models.ResponseModels;
using PRN232.NMS.API.Models.ResponseModels.SystemAccountResponses;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;

namespace PRN232.NMS.API.Controllers
{
    [ApiController]
    [Route("api/system-accounts")]
    [Produces("application/json")]
    [Authorize]
    public class SystemAccountController : ControllerBase
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly IMapper _mapper;

        public SystemAccountController(ISystemAccountService systemAccountService, IMapper mapper)
        {
            _systemAccountService = systemAccountService;
            _mapper = mapper;
        }

        //[HttpPost("authentication")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        //{
        //    var userAccount = await _systemAccountService.GetUserAccount(loginRequest.Username, loginRequest.Password);

        //    var mappedUser = _mapper.Map<LoginResponse>(userAccount);


        //    if (mappedUser != null)
        //    {
        //        var response = new ResponseDTO<LoginResponse>(message: "Login successfully", isSuccess: true, data: mappedUser, errors: null);

        //        return Ok(response);
        //    }

        //    return Unauthorized(new ResponseDTO<LoginResponse>(message: "Failed", isSuccess: true, data: null, errors: null));
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _systemAccountService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ResponseDTO<UserResponse>(message: "User not found", isSuccess: false, data: null, errors: null));
            }

            var mappedUser = _mapper.Map<UserResponse>(user);
            return Ok(new ResponseDTO<UserResponse>(message: "User retrieved successfully", isSuccess: true, data: mappedUser, errors: null));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, [FromQuery] string? sortBy = null, [FromQuery] bool isDescending = false)
        {
            var pagedResult = await _systemAccountService.GetUsersPagedAsync(page, pageSize, searchTerm, sortBy, isDescending);
            var mappedItems = _mapper.Map<List<UserResponse>>(pagedResult.Items);

            var pagedResponse = new PagedResult<UserResponse>
            {
                Items = mappedItems,
                Page = page,
                PageSize = pageSize,
                TotalItems = pagedResult.TotalItems,
                TotalPages = (int)Math.Ceiling(pagedResult.TotalItems / (double)pageSize)
            };

            return Ok(new ResponseDTO<PagedResult<UserResponse>>(message: "Users retrieved successfully", isSuccess: true, data: pagedResponse, errors: null));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSystemAccountRequest request)
        {
            var entity = _mapper.Map<SystemAccount>(request);
            await _systemAccountService.CreateUserAsync(entity);
            var responseData = _mapper.Map<UserResponse>(entity);

            return StatusCode(201, new ResponseDTO<UserResponse>(message: "User created successfully", isSuccess: true, data: responseData, errors: null));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _systemAccountService.DeleteUserAsync(id);

            if (result != "Delete successful")
            {
                return NotFound(new ResponseDTO<UserResponse>(message: $"User deletion failed: {result}", isSuccess: false, data: null, errors: null));
            }

            return Ok(new ResponseDTO<UserResponse>(message: "User deleted successfully", isSuccess: true, data: null, errors: null));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSystemAccountRequest request)
        {
            var entity = _mapper.Map<SystemAccount>(request);
            var result = await _systemAccountService.UpdateUserAsync(id, entity);

            if (result != "Update successful")
            {
                return NotFound(new ResponseDTO<UserResponse>(message: $"User modification failed: {result}", isSuccess: false, data: null, errors: null));
            }
            entity.AccountId = id;
            var responseData = _mapper.Map<UserResponse>(entity);

            return Ok(new ResponseDTO<UserResponse>(message: "User updated successfully", isSuccess: true, data: responseData, errors: null));
        }
    }
}
