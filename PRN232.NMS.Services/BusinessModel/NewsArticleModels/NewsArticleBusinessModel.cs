using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.NMS.Services.BusinessModel.NewsArticleModels
{
    public class NewsArticleBusinessModel
    {
        public int NewsArticleId { get; set; }

        public string NewsTitle { get; set; } = null!;

        public string? Headline { get; set; }

        public DateTime CreatedDate { get; set; }

        public string NewsContent { get; set; } = null!;

        public string? NewsSource { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public int NewsStatusId { get; set; }

        public int CreatedById { get; set; }

        public string CreatedByName { get; set; } = null!;

        public DateTime? ModifiedDate { get; set; }

        public int? UpdatedById { get; set; }

        public List<string> Tags { get; set; } = new();
    }
}
