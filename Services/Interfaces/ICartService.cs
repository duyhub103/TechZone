using MyWeb.ViewModels;

namespace MyWeb.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartViewModel> GetCartByUserIdAsync(string userId);
        Task AddToCartAsync(string userId, int productId, int quantity);
        Task ClearCartByUserAsync(string userId);
    }

}

