using EVCMS.Repositories.BinhLS.Basic;
using Microsoft.EntityFrameworkCore;
using PRN232.NMS.Repo.DBContext;
using PRN232.NMS.Repo.EntityModels;

namespace PRN232.NMS.Repo.Repositories
{
    public class SystemAccountRepository : GenericRepository<SystemAccount>
    {
        public SystemAccountRepository() { }
        public SystemAccountRepository(Prn312classDbContext context) : base(context) { }

        public async Task<SystemAccount?> GetByUsernameAsync(string username)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(ua => ua.AccountName == username);
        }

        public async Task<SystemAccount?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.SystemAccounts
                .Where(ua => ua.AccountId == id)
                .Select(ua => new SystemAccount
                {
                    AccountId = ua.AccountId,
                    AccountName = ua.AccountName,
                    AccountEmail = ua.AccountEmail,
                    AccountRole = ua.AccountRole,

                    NewsArticleCreatedBies = ua.NewsArticleCreatedBies.Select(a => new NewsArticle
                    {
                        NewsArticleId = a.NewsArticleId,
                        NewsTitle = a.NewsTitle,
                        Headline = a.Headline,
                        CreatedDate = a.CreatedDate
                    }).ToList(),

                    NewsArticleUpdatedBies = ua.NewsArticleUpdatedBies.Select(a => new NewsArticle
                    {
                        NewsArticleId = a.NewsArticleId,
                        NewsTitle = a.NewsTitle,
                        Headline = a.Headline,
                        CreatedDate = a.CreatedDate
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(List<SystemAccount> Items, int TotalItems)> GetAllPagedAsync(
            int skip,
            int take,
            string? searchTerm,
            string? sortBy,
            bool isDescending)
        {
            var query = _context.SystemAccounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.AccountName.Contains(searchTerm) || x.AccountEmail.Contains(searchTerm));
            }

            query = sortBy?.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(x => x.AccountName) : query.OrderBy(x => x.AccountName),
                "email" => isDescending ? query.OrderByDescending(x => x.AccountEmail) : query.OrderBy(x => x.AccountEmail),
                "role" => isDescending ? query.OrderByDescending(x => x.AccountRole) : query.OrderBy(x => x.AccountRole),
                _ => isDescending ? query.OrderByDescending(x => x.AccountId) : query.OrderBy(x => x.AccountId)
            };

            var totalItems = await query.CountAsync();

            var items = await query
                .Select(ua => new SystemAccount
                {
                    AccountId = ua.AccountId,
                    AccountName = ua.AccountName,
                    AccountEmail = ua.AccountEmail,
                    AccountRole = ua.AccountRole
                })
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return (items ?? new List<SystemAccount>(), totalItems);
        }

        public async Task<int> CountAsync()
        {
            return await _context.SystemAccounts.CountAsync();
        }
    }
}
