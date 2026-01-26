using AutoMapper;
using PRN232.NMS.Repo;
using PRN232.NMS.Services.DataHandler.ExceptionMidleware;
using PRN232.NMS.Services.Interfaces;
using PRN232.NMS.Services.Models.RequestModels.Auth;
namespace PRN232.NMS.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<string> LoginAsync(LoginRequestModel request)
        {
            var user = await _unitOfWork.SystemUserAccountRepository.LoginAsync(request.Email, request.Password);
            if (user == null)
            {
                throw new Exception("Invalid email or pasword");
            }
            else
                return _jwtService.GenerateToken(user);
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
