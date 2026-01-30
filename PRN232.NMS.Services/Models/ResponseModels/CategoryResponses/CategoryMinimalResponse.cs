namespace PRN232.NMS.Services.Models.ResponseModels.CategoryResponses
{
    /// <summary>
    /// Dùng cho fields=minimal (dropdown, listing gọn).
    /// </summary>
    public class CategoryMinimalResponse
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
