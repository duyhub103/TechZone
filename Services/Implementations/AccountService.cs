using Microsoft.AspNetCore.Identity;
using MyWeb.ViewModels;
using MyWeb.Services.Interfaces;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MyWeb.Models;

namespace MyWeb.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<(bool Success, string? Error)> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return (false, "Email hoặc mật khẩu không đúng");

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (!result.Succeeded) return (false, "Email hoặc mật không đúng");

            return (true, null);
        }



        public async Task<(bool Success, string? Error)> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var error = result.Errors.FirstOrDefault()?.Description;
                return (false, error);
            }

            // tự động login khi đăng ký
            await _signInManager.SignInAsync(user, isPersistent: false);
            return (true, null);

        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
