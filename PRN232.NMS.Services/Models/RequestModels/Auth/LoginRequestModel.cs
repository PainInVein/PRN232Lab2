using System.ComponentModel.DataAnnotations;

namespace PRN232.NMS.Services.Models.RequestModels.Auth
{
    public class LoginRequestModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
