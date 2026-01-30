using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.NMS.Services.Models
{
    public class RegisterRequestModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Length(3, 50)]
        public string Name { get; set; }
        [Required]
        [Length(8, 100)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
