using PRN232.NMS.Services.Models.RequestModels;
using System.ComponentModel.DataAnnotations;

namespace PRN232.NMS.Services.Models.RequestModels.NewsArticleRequests
{
    public class NewsArticleFilterRequest : PagedRequest
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? NewsStatusId { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
    }
}