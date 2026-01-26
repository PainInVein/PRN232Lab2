using EVCMS.Repositories.BinhLS.Basic;
using Microsoft.EntityFrameworkCore;
using PRN232.NMS.Repo.DBContext;
using PRN232.NMS.Repo.EntityModels;

namespace PRN232.NMS.Repo.Repositories
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository() { }

        public CategoryRepository(Prn312classDbContext context) : base(context) { }

        /// <summary>
        /// GET đưa FE: Select projection, giới hạn độ sâu. Không Include (tránh circular, lấy hết).
        /// Kiểm soát độ sâu dữ liệu - Tránh Circular Reference (Yêu cầu 4).
        /// </summary>
        public async Task<Category?> GetByCategoryIdAsync(int id)
        {
            return await _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new Category
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                    ParentCategoryId = c.ParentCategoryId,
                    IsActive = c.IsActive,
                    ParentCategory = c.ParentCategory == null ? null : new Category
                    {
                        CategoryId = c.ParentCategory.CategoryId,
                        CategoryName = c.ParentCategory.CategoryName
                    },
                    InverseParentCategory = c.InverseParentCategory
                        .Select(ch => new Category { CategoryId = ch.CategoryId, CategoryName = ch.CategoryName })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Lấy entity đầy đủ (Include) chỉ khi cần update/delete. Không dùng cho GET đưa FE.
        /// </summary>
        public async Task<Category?> GetByIdForUpdateAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.InverseParentCategory)
                .Include(c => c.NewsArticles)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        /// <summary>
        /// Tìm category theo tên (không phân biệt hoa thường). Dùng kiểm tra trùng khi tạo, giống TagRepository.GetByNameAsync.
        /// </summary>
        public async Task<Category?> GetByNameAsync(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return null;

            return await _context.Categories
                .Where(c => c.CategoryName.Trim().ToLower() == categoryName.Trim().ToLower())
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Lấy tất cả category đang active (IsActive = true), sắp xếp theo tên.
        /// </summary>
        public async Task<List<Category>> GetAllActiveAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách category gốc (ParentCategoryId = null). Hữu ích cho hierarchy.
        /// </summary>
        public async Task<List<Category>> GetRootCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == null)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy tất cả category có sắp xếp theo tên. Dùng cho dropdown, danh sách hiển thị.
        /// </summary>
        public async Task<List<Category>> GetAllOrderedAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        /// <summary>
        /// Tìm kiếm theo tên hoặc mô tả. Giống TagRepository.SearchAsync.
        /// </summary>
        public async Task<List<Category>> SearchAsync(string? searchTerm)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(c =>
                    c.CategoryName.Contains(term) ||
                    (c.CategoryDescription != null && c.CategoryDescription.Contains(term)));
            }

            return await query
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        /// <summary>
        /// GET Collection: search, filter, sort, paging. RESTful list API.
        /// </summary>
        public async Task<(List<Category> Items, int TotalItems)> GetPagedAsync(
            string? searchTerm,
            bool? isActive,
            int? parentCategoryId,
            string? sortBy,
            string? sortOrder,
            int page,
            int pageSize)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(c =>
                    c.CategoryName.Contains(term) ||
                    (c.CategoryDescription != null && c.CategoryDescription.Contains(term)));
            }

            if (isActive.HasValue)
                query = query.Where(c => c.IsActive == isActive.Value);

            if (parentCategoryId.HasValue)
                query = query.Where(c => c.ParentCategoryId == parentCategoryId.Value);

            var order = (sortOrder ?? "asc").Trim().ToLowerInvariant();
            query = (sortBy ?? "categoryName").Trim().ToLowerInvariant() switch
            {
                "categoryname" => order == "desc" ? query.OrderByDescending(c => c.CategoryName) : query.OrderBy(c => c.CategoryName),
                "isactive" => order == "desc" ? query.OrderByDescending(c => c.IsActive) : query.OrderBy(c => c.IsActive),
                "categoryid" => order == "desc" ? query.OrderByDescending(c => c.CategoryId) : query.OrderBy(c => c.CategoryId),
                _ => query.OrderBy(c => c.CategoryName)
            };

            var totalItems = await query.CountAsync();
            var skip = (page - 1) * pageSize;
            var items = await query.Skip(skip).Take(pageSize).ToListAsync();
            return (items, totalItems);
        }
    }
}
