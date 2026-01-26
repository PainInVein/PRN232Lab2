using PRN232.NMS.Services.Models.RequestModels;
using System.ComponentModel.DataAnnotations;

namespace PRN232.NMS.Services.Models.RequestModels.TagRequests
{
    public class TagFilterRequest : PagedRequest
    {
        public string? SearchName { get; set; }
        public string? SortOption { get; set; }
        public List<int>? NewArticleIds { get; set; }
    }
}
