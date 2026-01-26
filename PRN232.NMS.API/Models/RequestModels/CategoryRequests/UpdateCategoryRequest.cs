using System.ComponentModel.DataAnnotations;

namespace PRN232.NMS.API.Models.RequestModels.CategoryRequests
{
    public class UpdateCategoryRequest
    {
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Category name must be between 1 and 150 characters.")]
        public string CategoryName { get; set; } = null!;

        [StringLength(500)]
        public string? CategoryDescription { get; set; }

        public int? ParentCategoryId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
