using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.BusinessModel.CategoryModels;

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
        /// <summary> Trả về business model (CategoryWithRelated), không trả Entity. Theo mẫu ITagService.GetByIdAsync. </summary>
        Task<CategoryWithRelated?> GetByIdAsync(int id);
        Task CreateAsync(Category model);
        Task<string> DeleteAsync(int id);
        Task<string> UpdateAsync(int id, Category updatedCategory);
    }
}
