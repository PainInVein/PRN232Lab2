using PRN232.NMS.Services.Models;
using PRN232.NMS.Services.Models.RequestModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.NMS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<object> RegisterAsync(string email, string name, string password);
    }
}
