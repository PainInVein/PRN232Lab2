namespace PRN232.NMS.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task LogoutAsync();
        Task<object> RegisterAsync(string email, string name, string password);
    }
}
