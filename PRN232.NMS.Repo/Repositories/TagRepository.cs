using EVCMS.Repositories.BinhLS.Basic;
using Microsoft.EntityFrameworkCore;
using PRN232.NMS.Repo.DBContext;
using PRN232.NMS.Repo.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.NMS.Repo.Repositories
{
    public class TagRepository : GenericRepository<Tag>
    {
        public TagRepository() { }

        public TagRepository(Prn312classDbContext context) : base(context) { }

        public async Task<Tag?> GetByTagIdAsync(int id)
        {
            var tag = await _context.Tags
                .Where(t => t.TagId == id)
                .Select(t => new Tag
                {
                    TagId = t.TagId,
                    TagName = t.TagName,
                    Note = t.Note,

                    NewsArticles = t.NewsArticles.Select(a => new NewsArticle
                    {
                        NewsArticleId = a.NewsArticleId,
                        NewsTitle = a.NewsTitle,
                        Headline = a.Headline,
                        CreatedDate = a.CreatedDate,
                        NewsContent = a.NewsContent,
                        NewsSource = a.NewsSource,
                        CategoryId = a.CategoryId,
                        NewsStatusId = a.NewsStatusId,
                        CreatedById = a.CreatedById,
                        UpdatedById = a.UpdatedById
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return tag;
        }

        public async Task<Tag?> GetByIdWithArticlesAsync(int tagId)
        {
            return await _context.Tags
                .Where(t => t.TagId == tagId)
                .Select(t => new Tag
                {
                    TagId = t.TagId,
                    TagName = t.TagName,
                    Note = t.Note,

                    NewsArticles = t.NewsArticles.Select(a => new NewsArticle
                    {
                        NewsArticleId = a.NewsArticleId,
                        NewsTitle = a.NewsTitle,
                        Headline = a.Headline,
                        CreatedDate = a.CreatedDate,
                        NewsContent = a.NewsContent,
                        NewsSource = a.NewsSource,
                        CategoryId = a.CategoryId,
                        NewsStatusId = a.NewsStatusId,
                        CreatedById = a.CreatedById,
                        UpdatedById = a.UpdatedById
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Tag?> GetByNameAsync(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                return null;
            }

            return await _context.Tags
                .Where(t => t.TagName.Trim().ToLower() == tagName.Trim().ToLower())
                .Select(t => new Tag
                {
                    TagId = t.TagId,
                    TagName = t.TagName,
                    Note = t.Note,

                    NewsArticles = t.NewsArticles.Select(a => new NewsArticle
                    {
                        NewsArticleId = a.NewsArticleId,
                        NewsTitle = a.NewsTitle,
                        Headline = a.Headline,
                        CreatedDate = a.CreatedDate,
                        NewsContent = a.NewsContent,
                        NewsSource = a.NewsSource,
                        CategoryId = a.CategoryId,
                        NewsStatusId = a.NewsStatusId,
                        CreatedById = a.CreatedById,
                        UpdatedById = a.UpdatedById
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<Tag>> SearchAsync(string tagName)
        {
            var query = _context.Tags.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tagName))
            {
                query = query.Where(t => t.TagName.Contains(tagName));
            }

            var items = await query
                .Select(t => new Tag
                {
                    TagId = t.TagId,
                    TagName = t.TagName,
                    Note = t.Note,

                    NewsArticles = t.NewsArticles.Select(a => new NewsArticle
                    {
                        NewsArticleId = a.NewsArticleId,
                        NewsTitle = a.NewsTitle,
                        Headline = a.Headline,
                        CreatedDate = a.CreatedDate,
                        NewsContent = a.NewsContent,
                        NewsSource = a.NewsSource,
                        CategoryId = a.CategoryId,
                        NewsStatusId = a.NewsStatusId,
                        CreatedById = a.CreatedById,
                        UpdatedById = a.UpdatedById
                    }).ToList()
                })
                .OrderBy(t => t.TagName)
                .ToListAsync();

            return items ?? new List<Tag>();
        }

        // Optional: very lightweight version without articles
        public async Task<(List<Tag> Items, int TotalItems)> GetAllSimpleAsync(int skip, int take, string? searchTerm, string? sortOption, List<int>? newArticleIds)
        {
            var query = _context.Tags.AsQueryable();

            if (newArticleIds != null && newArticleIds.Count > 0)
            {
                query = query.Where(t =>
                    newArticleIds.All(id =>
                        t.NewsArticles.Any(na => na.NewsArticleId == id)
                    )
                );
            }


            //Search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.TagName.Contains(searchTerm));
            }

            //Sort
            query = sortOption?.ToLower() switch
            {
                "desc" => query.OrderByDescending(x => x.TagName),
                "asc" => query.OrderBy(x => x.TagName),
                _ => query.OrderBy(x => x.TagId)
            };


            var items = await query
                .Select(t => new Tag
                {
                    TagId = t.TagId,
                    TagName = t.TagName,
                    Note = t.Note,
                })
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalItems = await query.CountAsync();


            return (items ?? new List<Tag>(), totalItems);
        }

        public async Task<int> CountAsync()
        {
            var result = await _context.Tags.CountAsync();

            return result;
        }
    }
}
