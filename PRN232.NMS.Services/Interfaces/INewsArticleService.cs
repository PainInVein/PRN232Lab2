using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.BusinessModel.NewsArticleModels;

namespace PRN232.NMS.Services.Interfaces
{
    public interface INewsArticleService
    {
        Task<(List<NewsArticleBusinessModel> Items, int TotalItems)> GetAllPagedAsync(
        string? searchTerm, int? categoryId, int? statusId, string? sortColumn, string? sortOrder, int page, int pageSize);
        Task<NewsArticleBusinessModel?> GetByIdAsync(int id);
        Task CreateAsync(NewsArticle article, List<int>? tagIds);
        Task UpdateAsync(int id, NewsArticle article, List<int>? tagIds);
        Task DeleteAsync(int id);
    }
}