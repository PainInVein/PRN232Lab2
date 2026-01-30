using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.BusinessModel.TagModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.NMS.Services.Interfaces
{
    public interface ITagService
    {
        Task<TagWithNewsArticle?> GetByIdAsync(int id);

        Task<(List<Tag> Items, int TotalItems)> GetTagsPagedAsync(int page, int pageSize, string? searchTerm, string? sortOption, List<int>? newArticleIds);

        Task CreateTagAsync(Tag tag);

        Task<string> DeleteTagAsync(int id);

        Task<string> UpdateTagAsync(int id, Tag updatedTag);
    }
}
