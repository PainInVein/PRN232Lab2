using System;
using System.Collections.Generic;

namespace PRN232.NMS.Services.BusinessModel.SystemAccountModels
{
    public class SystemAccountBusinessModel
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; } = null!;

        public string AccountEmail { get; set; } = null!;

        public string AccountRole { get; set; } = null!;

    }

    public class AccountRelatedNewsArticleModel
    {
        public int NewsArticleId { get; set; }
        public string NewsTitle { get; set; } = null!;
        public string? Headline { get; set; }
        public DateTime CreatedDate { get; set; }
        public int NewsStatusId { get; set; }
    }
}