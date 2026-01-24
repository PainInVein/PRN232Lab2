using PRN232.NMS.Repo.EntityModels;

namespace PRN232.NMS.Services.Interfaces
{
    public interface INewsArticleService
    {
        Task<(List<NewsArticle> Items, int TotalItems)> GetAllPagedAsync(
            string? searchTerm, int? categoryId, int? statusId, string? sortColumn, string? sortOrder, int page, int pageSize);
        Task<NewsArticle?> GetByIdAsync(int id);
        Task CreateAsync(NewsArticle article, List<int>? tagIds);
        Task UpdateAsync(int id, NewsArticle article, List<int>? tagIds);
        Task DeleteAsync(int id);
    }
}