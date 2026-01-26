using System.ComponentModel.DataAnnotations;

namespace PRN232.NMS.Services.Models.RequestModels.TagRequests
{
    public class UpdateTagRequest
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tag name must be between 2 and 50 characters.")]
        public string TagName { get; set; } = null!;

        [StringLength(200, MinimumLength = 3, ErrorMessage = "Note cannot exceed 200 characters.")]
        public string? Note { get; set; }
    }
}
