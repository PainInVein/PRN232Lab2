using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;
using Repositories;

namespace PRN232.NMS.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var list = await _unitOfWork.CategoryRepository.GetAllOrderedAsync();
            return list;
        }

        public async Task<(IEnumerable<Category> Items, int TotalItems)> GetCategoriesPagedAsync(
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            int? parentCategoryId,
            string? sortBy,
            string? sortOrder)
        {
            var (items, totalItems) = await _unitOfWork.CategoryRepository.GetPagedAsync(
                searchTerm, isActive, parentCategoryId, sortBy, sortOrder, page, pageSize);
            return (items, totalItems);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _unitOfWork.CategoryRepository.GetByCategoryIdAsync(id);
        }

        public async Task CreateAsync(Category model)
        {
            await _unitOfWork.CategoryRepository.CreateAsync(model);
        }

        public async Task<string> DeleteAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdForUpdateAsync(id);
            if (category == null) return "Category not found";

            // Không cho delete nếu có NewsArticles (FK constraint)
            if (category.NewsArticles != null && category.NewsArticles.Any())
                return "Cannot delete category that has associated news articles.";

            // Không cho delete nếu có children (tránh orphan)
            if (category.InverseParentCategory != null && category.InverseParentCategory.Any())
                return "Cannot delete category that has child categories. Please delete or reassign children first.";

            await _unitOfWork.CategoryRepository.RemoveAsync(category);
            return string.Empty;
        }

        public async Task<string> UpdateAsync(int id, Category updatedCategory)
        {
            var existing = await _unitOfWork.CategoryRepository.GetByIdForUpdateAsync(id);
            if (existing == null) return "Category not found";

            // Validate: ParentCategoryId không được = chính nó (circular)
            if (updatedCategory.ParentCategoryId == id)
                return "Category cannot be its own parent.";

            // Validate: ParentCategoryId không được trỏ đến một trong các children (circular)
            if (updatedCategory.ParentCategoryId.HasValue && existing.InverseParentCategory != null)
            {
                var childIds = existing.InverseParentCategory.Select(c => c.CategoryId).ToList();
                if (childIds.Contains(updatedCategory.ParentCategoryId.Value))
                    return "Category cannot be a child of its own descendant (circular reference).";
            }

            // Validate: ParentCategoryId phải tồn tại (nếu có)
            if (updatedCategory.ParentCategoryId.HasValue)
            {
                var parent = await _unitOfWork.CategoryRepository.GetByIdAsync(updatedCategory.ParentCategoryId.Value);
                if (parent == null)
                    return "Parent category not found.";
            }

            existing.CategoryName = updatedCategory.CategoryName;
            existing.CategoryDescription = updatedCategory.CategoryDescription;
            existing.ParentCategoryId = updatedCategory.ParentCategoryId;
            existing.IsActive = updatedCategory.IsActive;

            await _unitOfWork.CategoryRepository.UpdateAsync(existing);
            return string.Empty;
        }
    }
}
