using PRN232.NMS.Repo;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Repo.Repositories;
using PRN232.NMS.Services.Interfaces;

namespace PRN232.NMS.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly UnitOfWork _unitOfWork;

        public NewsArticleService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<(List<NewsArticle> Items, int TotalItems)> GetAllPagedAsync(
            string? searchTerm, int? categoryId, int? statusId, string? sortColumn, string? sortOrder, int page, int pageSize)
        {
            return await _unitOfWork.NewsArticleRepository.GetPagedAsync(searchTerm, categoryId, statusId, sortColumn, sortOrder, page, pageSize);
        }

        public async Task<NewsArticle?> GetByIdAsync(int id)
        {
            return await _unitOfWork.NewsArticleRepository.GetByIdDetailedAsync(id);
        }

        public async Task CreateAsync(NewsArticle article, List<int>? tagIds)
        {
            article.CreatedDate = DateTime.Now;
            article.ModifiedDate = DateTime.Now;
            article.NewsStatusId = 1;

            if (tagIds != null && tagIds.Any())
            {
                foreach (var tagId in tagIds)
                {
                    var tag = await _unitOfWork.TagRepository.GetByIdAsync(tagId);
                    if (tag != null) article.Tags.Add(tag);
                }
            }

            await _unitOfWork.NewsArticleRepository.CreateAsync(article);
        }

        public async Task UpdateAsync(int id, NewsArticle updatedArticle, List<int>? tagIds)
        {
            var existingArticle = await _unitOfWork.NewsArticleRepository.GetByIdDetailedAsync(id);
            if (existingArticle == null) throw new KeyNotFoundException("Article not found");

            existingArticle.NewsTitle = updatedArticle.NewsTitle;
            existingArticle.Headline = updatedArticle.Headline;
            existingArticle.NewsContent = updatedArticle.NewsContent;
            existingArticle.NewsSource = updatedArticle.NewsSource;
            existingArticle.CategoryId = updatedArticle.CategoryId;
            existingArticle.NewsStatusId = 1;
            existingArticle.ModifiedDate = DateTime.Now;
            existingArticle.UpdatedById = updatedArticle.UpdatedById;

            if (tagIds != null)
            {
                existingArticle.Tags.Clear();
                foreach (var tagId in tagIds)
                {
                    var tag = await _unitOfWork.TagRepository.GetByIdAsync(tagId);
                    if (tag != null) existingArticle.Tags.Add(tag);
                }
            }

            await _unitOfWork.NewsArticleRepository.UpdateAsync(existingArticle);
        }

        public async Task DeleteAsync(int id)
        {
            var article = await _unitOfWork.NewsArticleRepository.GetByIdAsync(id);
            if (article == null) throw new KeyNotFoundException("Article not found");
            article.NewsStatusId = 2;

            await _unitOfWork.NewsArticleRepository.UpdateAsync(article);
        }
    }
}