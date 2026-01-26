using PRN232.NMS.Repo.EntityModels;

namespace PRN232.NMS.Services.Interfaces
{
    public interface ISystemAccountService
    {
        Task<SystemAccount?> GetUserAccount(string username, string password);
        Task<SystemAccount?> GetByIdAsync(int id);
        Task<(List<SystemAccount> Items, int TotalItems)> GetUsersPagedAsync(int page, int pageSize, string? searchTerm, string? sortBy, bool isDescending);
        Task CreateUserAsync(SystemAccount account);
        Task<string> DeleteUserAsync(int id);
        Task<string> UpdateUserAsync(int id, SystemAccount updatedAccount);
    }
}
