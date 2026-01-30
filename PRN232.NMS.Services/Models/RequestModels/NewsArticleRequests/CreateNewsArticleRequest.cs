using System.ComponentModel.DataAnnotations;

namespace PRN232.NMS.Services.Models.RequestModels.NewsArticleRequests
{
    public class CreateNewsArticleRequest
    {
        [Required(ErrorMessage = "News Title is required")]
        [StringLength(255)]
        public string NewsTitle { get; set; } = null!;

        public string? Headline { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string NewsContent { get; set; } = null!;

        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tags are required")]
        public List<int> TagIds { get; set; } = new();
    }
}