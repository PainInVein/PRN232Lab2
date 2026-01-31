namespace PRN232.NMS.Services.BusinessModel.CategoryModels
{
    /// <summary>
    /// Business model cho Category khi GET by Id: đầy đủ thông tin + ParentCategoryName + danh sách con (tránh circular ref).
    /// Dùng trong Service layer, tách biệt với Entity. Theo mẫu TagModels (TagWithNewsArticle).
    /// </summary>
    public class CategoryWithRelated
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? CategoryDescription { get; set; }
        public int? ParentCategoryId { get; set; }
        /// <summary> Tên category cha (hiển thị, không cần load thêm). </summary>
        public string? ParentCategoryName { get; set; }
        public bool IsActive { get; set; }
        /// <summary> Danh sách category con (chỉ Id + Name, tránh đệ quy). </summary>
        public List<CategoryMinimalBusinessModel> ChildCategories { get; set; } = new();
    }

    /// <summary>
    /// Model gọn cho category con / dropdown. Dùng trong ChildCategories của CategoryWithRelated.
    /// </summary>
    public class CategoryMinimalBusinessModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
