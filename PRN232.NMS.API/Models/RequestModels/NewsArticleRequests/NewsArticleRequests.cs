using System.ComponentModel.DataAnnotations;

namespace PRN232.NMS.API.Models.RequestModels.NewsArticleRequests
{
    public class NewsArticleFilterRequest : PagedRequest
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? NewsStatusId { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
    }

    public class CreateNewsArticleRequest
    {
        [Required(ErrorMessage = "News Title is required")]
        [StringLength(255)]
        public string NewsTitle { get; set; } = null!;

        public string? Headline { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string NewsContent { get; set; } = null!;

        public string? NewsSource { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<int>? TagIds { get; set; }
    }

    public class UpdateNewsArticleRequest
    {
        [Required]
        [StringLength(255)]
        public string NewsTitle { get; set; } = null!;

        public string? Headline { get; set; }

        [Required]
        public string NewsContent { get; set; } = null!;

        public string? NewsSource { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<int>? TagIds { get; set; }
    }
}
