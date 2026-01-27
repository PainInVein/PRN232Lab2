using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using PRN232.NMS.Repo;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;
using PRN232.NMS.Services.Models;
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
                return null;
            }
            else
                return _jwtService.GenerateToken(user);
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<object> RegisterAsync(RegisterRequestModel request)
        {
            var existingEmail = await _unitOfWork.SystemUserAccountRepository.IsEmailExist(request.Email);
            if (existingEmail)
            {
                throw new Exception("Email already exists");                
            }
            var userAccount = _mapper.Map<SystemAccount>(request);
            userAccount.AccountRole = "Reporter";
            userAccount.AccountEmail = request.Email;
            userAccount.AccountPassword = request.Password;
            userAccount.AccountName = request.Name;

            await _unitOfWork.SystemUserAccountRepository.CreateAsync(userAccount);
            await _unitOfWork.SaveChangeWithTransactionAsync();

            return request;
        }
    }
}
