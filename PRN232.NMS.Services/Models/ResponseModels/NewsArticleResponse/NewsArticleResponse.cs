namespace PRN232.NMS.Services.Models.ResponseModels.NewsArticleResponse
{
    public class NewsArticleResponse
    {
        public int NewsArticleId { get; set; }
        public string NewsTitle { get; set; } = null!;
        public string? Headline { get; set; }
        public DateTime CreatedDate { get; set; }
        public string NewsContent { get; set; } = null!;
        public string? NewsSource { get; set; }
        public string CategoryName { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int NewsStatusId { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
