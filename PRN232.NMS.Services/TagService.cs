using PRN232.NMS.Repo;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;

namespace PRN232.NMS.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService() => _unitOfWork ??= new UnitOfWork();

        public async Task<Tag?> GetByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.TagRepository.GetByTagIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n");
            }
            return null;
        }

        public async Task<(List<Tag> Items, int TotalItems)> GetTagsPagedAsync(int page, int pageSize, string? searchTerm, string? sortOption, List<int>? newArticleIds)
        {
            try
            {
                var items = await _unitOfWork.TagRepository
                    .GetAllSimpleAsync((page - 1) * pageSize, pageSize, searchTerm, sortOption, newArticleIds);


                return (items.Items, items.TotalItems);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateTagAsync(Tag tag)
        {
             await _unitOfWork.TagRepository.CreateAsync(tag);
            
        }

        public async Task<string> DeleteTagAsync(int id)
        {
            var tag = await _unitOfWork.TagRepository.GetByIdAsync(id);
            if (tag == null) return "Tag not found";
            _unitOfWork.TagRepository.Remove(tag);
            return string.Empty;
        }

        public async Task<string> UpdateTagAsync(int id, Tag updatedTag)
        {
            var existingTag = await _unitOfWork.TagRepository.GetByIdAsync(id);
            if (existingTag == null) return "Tag not found";
            existingTag.TagName = updatedTag.TagName;
            existingTag.Note = updatedTag.Note;
            _unitOfWork.TagRepository.Update(existingTag);
            return string.Empty;
        }
    }
}
