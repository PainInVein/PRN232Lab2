using AutoMapper;
using PRN232.NMS.Repo;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.BusinessModel.SystemAccountModels;
using PRN232.NMS.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace PRN232.NMS.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SystemAccountService(IMapper mapper)
        {
            _unitOfWork = new UnitOfWork();
            _mapper = mapper;
        }

        public async Task<SystemAccount?> GetUserAccount(string username, string password)
        {
            var user = await _unitOfWork.SystemUserAccountRepository.GetByUsernameAsync(username);

            if (user != null && BC.Verify(password, user.AccountPassword))
            {
                return user;
            }

            return null;

        }
        public async Task<SystemAccountBusinessModel?> GetByIdAsync(int id)
        {
            var userEntity = await _unitOfWork.SystemUserAccountRepository.GetByIdAsync(id);
            if (userEntity == null) return null;
            return _mapper.Map<SystemAccountBusinessModel>(userEntity);
        }

        public async Task<(List<SystemAccountBusinessModel> Items, int TotalItems)> GetUsersPagedAsync(
            int page, int pageSize, string? searchTerm, string? sortBy, bool isDescending)
        {
            int skip = (page - 1) * pageSize;
            var result = await _unitOfWork.SystemUserAccountRepository.GetAllPagedAsync(
                skip, pageSize, searchTerm, sortBy, isDescending);
            var businessModels = _mapper.Map<List<SystemAccountBusinessModel>>(result.Items);

            return (businessModels, result.TotalItems);
        }

        public async Task CreateUserAsync(SystemAccount account)
        {
            if (!string.IsNullOrEmpty(account.AccountPassword))
            {
                account.AccountPassword = BC.HashPassword(account.AccountPassword);
            }
            await _unitOfWork.SystemUserAccountRepository.CreateAsync(account);
            await _unitOfWork.SaveChangeWithTransactionAsync();
        }

        public async Task<string> UpdateUserAsync(int id, SystemAccount updatedAccount)
        {
            var existingUser = await _unitOfWork.SystemUserAccountRepository.GetByIdAsync(id);
            if (existingUser == null) return "User not found";

            existingUser.AccountName = updatedAccount.AccountName;
            existingUser.AccountEmail = updatedAccount.AccountEmail;
            existingUser.AccountRole = updatedAccount.AccountRole;
            if (!string.IsNullOrEmpty(updatedAccount.AccountPassword))
            {
                existingUser.AccountPassword = BC.HashPassword(updatedAccount.AccountPassword);
            }

            _unitOfWork.SystemUserAccountRepository.Update(existingUser);
            try
            {
                await _unitOfWork.SaveChangeWithTransactionAsync();
                return "Update successful";
            }
            catch (Exception ex)
            {
                return $"Update failed: {ex.Message}";
            }
        }

        public async Task<string> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.SystemUserAccountRepository.GetByIdAsync(id);
            if (user == null) return "User not found";

            _unitOfWork.SystemUserAccountRepository.Remove(user);
            var result = await _unitOfWork.SaveChangeWithTransactionAsync();

            try
            {
                await _unitOfWork.SaveChangeWithTransactionAsync();
                return "Delete successful";
            }
            catch (Exception ex)
            {
                return $"Delete failed: {ex.InnerException?.Message ?? ex.Message}";
            }
        }
    }
}
