using PRN232.NMS.API.Models.RequestModels;

namespace PRN232.NMS.API.Models.RequestModels.CategoryRequests
{
    /// <summary>
    /// Query params cho GET /api/categories. camelCase, resource-based.
    /// Search, filtering, sorting, paging, field selection.
    /// </summary>
    public class CategoryFilterRequest : PagedRequest
    {
        /// <summary> Tìm theo tên hoặc mô tả. </summary>
        public string? SearchTerm { get; set; }

        /// <summary> Lọc theo trạng thái active. </summary>
        public bool? IsActive { get; set; }

        /// <summary> Lọc theo category cha (null = root). </summary>
        public int? ParentCategoryId { get; set; }

        /// <summary> Sort theo: categoryName, isActive. Mặc định categoryName. </summary>
        public string? SortBy { get; set; }

        /// <summary> asc | desc. Mặc định asc. </summary>
        public string? SortOrder { get; set; }

        /// <summary> minimal = chỉ id + name; full hoặc bỏ qua = đầy đủ. </summary>
        public string? Fields { get; set; }
    }
}
