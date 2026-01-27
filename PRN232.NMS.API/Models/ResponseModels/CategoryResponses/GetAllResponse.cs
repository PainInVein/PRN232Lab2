namespace PRN232.NMS.API.Models.ResponseModels.CategoryResponses
{
    public class GetAllResponse
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? CategoryDescription { get; set; }

        public int? ParentCategoryId { get; set; }

        public bool IsActive { get; set; }
    }
}
