using PRN232.NMS.Repo;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace PRN232.NMS.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemAccountService() => _unitOfWork ??= new UnitOfWork();

        public async Task<SystemAccount?> GetUserAccount(string username, string password)
        {
            var user = await _unitOfWork.SystemUserAccountRepository.GetByUsernameAsync(username);

            if (user != null && BC.Verify(password, user.AccountPassword))
            {
                return user;
            }

            return null;

        }
        public async Task<SystemAccount?> GetByIdAsync(int id)
        {
            return await _unitOfWork.SystemUserAccountRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<(List<SystemAccount> Items, int TotalItems)> GetUsersPagedAsync(
            int page, int pageSize, string? searchTerm, string? sortBy, bool isDescending)
        {
            int skip = (page - 1) * pageSize;
            return await _unitOfWork.SystemUserAccountRepository.GetAllPagedAsync(
                skip, pageSize, searchTerm, sortBy, isDescending);
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
