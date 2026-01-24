using System.ComponentModel.DataAnnotations;

namespace PRN232.NMS.API.Models.RequestModels.TagRequests
{
    public class TagFilterRequest : PagedRequest
    {
        [Required(ErrorMessage ="bye bye")]
        public string? SearchName { get; set; }
        [Required(ErrorMessage = "bye byedsadas")]
        public string? SortOption { get; set; }
        [Required(ErrorMessage = "bye bye")]
        public List<int>? NewArticleIds { get; set; }
    }
}
