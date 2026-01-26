using System.ComponentModel.DataAnnotations;

namespace PRN232.NMS.API.Models.RequestModels.SystemAccountRequests
{
    public class UpdateSystemAccountRequest
    {
        [Required(ErrorMessage = "Account name is required")]
        public string AccountName { get; set; } = null!;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string AccountEmail { get; set; } = null!;

        [Required]
        [RegularExpression("^(Admin|Editor|Reporter)$",
            ErrorMessage = "Role must be 'Admin', 'Editor' or 'Reporter'.")]
        public string AccountRole { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string AccountPassword { get; set; } = null!;
    }
}
