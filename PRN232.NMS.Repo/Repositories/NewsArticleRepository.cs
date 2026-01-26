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
                .Where(n => n.NewsArticleId == id)
                .Select(n => new NewsArticle
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    Headline = n.Headline,
                    NewsContent = n.NewsContent,
                    NewsSource = n.NewsSource,
                    CreatedDate = n.CreatedDate,
                    ModifiedDate = n.ModifiedDate,
                    CategoryId = n.CategoryId,
                    NewsStatusId = n.NewsStatusId,
                    CreatedById = n.CreatedById,
                    UpdatedById = n.UpdatedById,
                    Category = new Category
                    {
                        CategoryId = n.Category.CategoryId,
                        CategoryName = n.Category.CategoryName
                    },
                    CreatedBy = new SystemAccount
                    {
                        AccountId = n.CreatedBy.AccountId,
                        AccountName = n.CreatedBy.AccountName,
                        AccountEmail = n.CreatedBy.AccountEmail
                    },
                    Tags = n.Tags.Select(t => new Tag
                    {
                        TagId = t.TagId,
                        TagName = t.TagName,
                        Note = t.Note
                    }).ToList()
                })
                .FirstOrDefaultAsync();
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
            var query = _context.NewsArticles.AsQueryable();

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

            var items = await query
                .Select(n => new NewsArticle
                {
                    NewsArticleId = n.NewsArticleId,
                    NewsTitle = n.NewsTitle,
                    Headline = n.Headline,
                    NewsContent = n.NewsContent,
                    NewsSource = n.NewsSource,
                    CreatedDate = n.CreatedDate,
                    CategoryId = n.CategoryId,
                    NewsStatusId = n.NewsStatusId,
                    CreatedById = n.CreatedById,
                    UpdatedById = n.UpdatedById,
                    Category = new Category
                    {
                        CategoryId = n.Category.CategoryId,
                        CategoryName = n.Category.CategoryName
                    },
                    CreatedBy = new SystemAccount
                    {
                        AccountId = n.CreatedBy.AccountId,
                        AccountName = n.CreatedBy.AccountName
                    },
                    Tags = n.Tags.Select(t => new Tag
                    {
                        TagId = t.TagId,
                        TagName = t.TagName
                    }).ToList()
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItems);
        }
    }
}