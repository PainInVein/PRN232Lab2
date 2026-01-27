using PRN232.NMS.Repo;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;

namespace PRN232.NMS.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

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
            if (id <= 0)
                throw new ArgumentException("Article ID must be greater than 0", nameof(id));

            return await _unitOfWork.NewsArticleRepository.GetByIdDetailedAsync(id);
        }

        public async Task CreateAsync(NewsArticle article, List<int>? tagIds)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article), "Article data cannot be null");

            if (string.IsNullOrWhiteSpace(article.NewsTitle))
                throw new ArgumentException("Article title is required", nameof(article.NewsTitle));

            if (article.CategoryId <= 0)
                throw new ArgumentException("Invalid category ID", nameof(article.CategoryId));

            // Check category exists
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(article.CategoryId);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {article.CategoryId} not found");

            article.CreatedDate = DateTime.Now;
            article.ModifiedDate = DateTime.Now;
            article.NewsStatusId = 1;

            // Process tags - fetch all at once
            if (tagIds != null && tagIds.Any())
            {
                var tags = await _unitOfWork.TagRepository.GetByIdsAsync(tagIds);

                if (tags.Count != tagIds.Count)
                {
                    var notFoundIds = tagIds.Except(tags.Select(t => t.TagId)).ToList();
                    throw new KeyNotFoundException($"Tags not found: {string.Join(", ", notFoundIds)}");
                }
                article.Tags = tags;
            }

            await _unitOfWork.NewsArticleRepository.CreateAsync(article);
        }

        public async Task UpdateAsync(int id, NewsArticle updatedArticle, List<int>? tagIds)
        {
            if (id <= 0)
                throw new ArgumentException("Article ID must be greater than 0", nameof(id));

            if (updatedArticle == null)
                throw new ArgumentNullException(nameof(updatedArticle), "Updated article data cannot be null");

            if (string.IsNullOrWhiteSpace(updatedArticle.NewsTitle))
                throw new ArgumentException("Article title is required", nameof(updatedArticle.NewsTitle));

            var existingArticle = await _unitOfWork.NewsArticleRepository.GetAllArticleForUpdate(id);
            if (existingArticle == null)
                throw new KeyNotFoundException($"Article with ID {id} not found");

            // Validate category
            if (updatedArticle.CategoryId != existingArticle.CategoryId)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(updatedArticle.CategoryId);
                if (category == null)
                    throw new KeyNotFoundException($"Category with ID {updatedArticle.CategoryId} not found");
            }

            existingArticle.NewsTitle = updatedArticle.NewsTitle;
            existingArticle.Headline = updatedArticle.Headline;
            existingArticle.NewsContent = updatedArticle.NewsContent;
            existingArticle.NewsSource = updatedArticle.NewsSource;
            existingArticle.CategoryId = updatedArticle.CategoryId;
            existingArticle.ModifiedDate = DateTime.Now;
            existingArticle.UpdatedById = updatedArticle.UpdatedById;

            if (tagIds != null)
            {
                existingArticle.Tags.Clear();

                if (tagIds.Any())
                {
                    var tags = await _unitOfWork.TagRepository.GetByIdsAsync(tagIds);

                    //if (tags.Count != tagIds.Count)
                    //{
                    //    var notFoundIds = tagIds.Except(tags.Select(t => t.TagId)).ToList();
                    //    throw new KeyNotFoundException($"Tags not found: {string.Join(", ", notFoundIds)}");
                    //}

                    foreach (var tag in tags)
                    {
                        existingArticle.Tags.Add(tag);
                    }
                }
            }

            await _unitOfWork.NewsArticleRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Article ID must be greater than 0", nameof(id));

            var article = await _unitOfWork.NewsArticleRepository.GetByIdAsync(id);
            if (article == null)
                throw new KeyNotFoundException($"Article with ID {id} not found");

            article.NewsStatusId = 2;
            await _unitOfWork.NewsArticleRepository.UpdateAsync(article);
        }
    }
}