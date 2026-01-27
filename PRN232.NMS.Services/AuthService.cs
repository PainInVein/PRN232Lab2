using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using PRN232.NMS.Repo;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;
using PRN232.NMS.Services.Models;
using PRN232.NMS.Services.Models.RequestModels.Auth;
using PRN232.NMS.Services.Models.ResponseModels.SystemAccountResponses;
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

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _unitOfWork.SystemUserAccountRepository.LoginAsync(email, password);
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

        public async Task<object> RegisterAsync(string email, string name, string password)
        {
            var existingEmail = await _unitOfWork.SystemUserAccountRepository.IsEmailExist(email);
            if (existingEmail)
            {
                throw new Exception("Email already exists");                
            }
            var userAccount = new SystemAccount()
            {
                AccountEmail = email,
                AccountName = name,
                AccountPassword = password,
                AccountRole = "Reporter"
            };

            await _unitOfWork.SystemUserAccountRepository.CreateAsync(userAccount);
            await _unitOfWork.SaveChangeWithTransactionAsync();

            return _mapper.Map<UserResponse>(userAccount);
        }
    }
}
