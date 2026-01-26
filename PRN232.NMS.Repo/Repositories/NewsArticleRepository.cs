using Microsoft.EntityFrameworkCore;
using PRN232.NMS.Repo.Basic;
using PRN232.NMS.Repo.DBContext;
using PRN232.NMS.Repo.EntityModels;
using System.Linq.Expressions;

namespace PRN232.NMS.Repo.Repositories
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>
    {
        public NewsArticleRepository() { }
        public NewsArticleRepository(Prn312classDbContext context) : base(context) { }

        public async Task<NewsArticle?> GetByIdDetailedAsync(int id)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }

        public async Task<(List<NewsArticle> Items, int TotalItems)> GetPagedAsync(
            string? searchTerm,
            int? categoryId,
            int? statusId,
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize)
        {
            var query = _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.NewsTitle.Contains(searchTerm) || x.NewsContent.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            if (statusId.HasValue)
            {
                query = query.Where(x => x.NewsStatusId == statusId);
            }

            query = sortColumn?.ToLower() switch
            {
                "newstitle" => sortOrder == "desc" ? query.OrderByDescending(x => x.NewsTitle) : query.OrderBy(x => x.NewsTitle),
                "createddate" => sortOrder == "desc" ? query.OrderByDescending(x => x.CreatedDate) : query.OrderBy(x => x.CreatedDate),
                _ => query.OrderByDescending(x => x.CreatedDate)
            };

            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalItems);
        }
    }
}