namespace PRN232.NMS.Services.Models.ResponseModels.CategoryResponses
{
    /// <summary>
    /// GET /api/categories/{id}. Đầy đủ thông tin liên quan, không circular ref.
    /// </summary>
    public class GetByIdResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? CategoryDescription { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public bool IsActive { get; set; }
        /// <summary> Danh sách con (chỉ id + name, tránh đệ quy). </summary>
        public List<CategoryMinimalResponse> ChildCategories { get; set; } = new();
    }
}
