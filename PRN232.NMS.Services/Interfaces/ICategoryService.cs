using PRN232.NMS.Repo.EntityModels;

namespace PRN232.NMS.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<(IEnumerable<Category> Items, int TotalItems)> GetCategoriesPagedAsync(
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            int? parentCategoryId,
            string? sortBy,
            string? sortOrder);
        Task<Category?> GetByIdAsync(int id);
        Task CreateAsync(Category model);
        Task<string> DeleteAsync(int id);
        Task<string> UpdateAsync(int id, Category updatedCategory);
    }
}
