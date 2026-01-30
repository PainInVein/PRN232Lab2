using AutoMapper;
using PRN232.NMS.Repo;
using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.BusinessModel.TagModels;
using PRN232.NMS.Services.Interfaces;

namespace PRN232.NMS.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TagWithNewsArticle?> GetByIdAsync(int id)
        {
            try
            {
                var serviceResult = await _unitOfWork.TagRepository.GetByTagIdAsync(id);

                var result = _mapper.Map<TagWithNewsArticle>(serviceResult);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n");
            }
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
