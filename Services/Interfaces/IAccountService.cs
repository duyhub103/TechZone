using MyWeb.ViewModels;

namespace MyWeb.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string? Error)> LoginAsync(LoginViewModel model);
        Task<(bool Success, string? Error)> RegisterAsync(RegisterViewModel model);
        Task LogoutAsync();
    }
}
