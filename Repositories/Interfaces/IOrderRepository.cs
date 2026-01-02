using MyWeb.Models;
using MyWeb.ViewModels;

namespace MyWeb.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        bool HasUserPurchasedProduct(string userId, int productId);
        //Task<int> CreateOrderAsync(Order order);
        Task<int> CreateOrderWithTransactionAsync(Order order, List<CartItemViewModel> cartItems);

        Task<Order?> GetOrderByIdAsync(int id);
    }
}
