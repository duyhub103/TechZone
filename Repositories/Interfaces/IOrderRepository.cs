using MyWeb.Models;

namespace MyWeb.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        bool HasUserPurchasedProduct(string userId, int productId);
        Task<int> CreateOrderAsync(Order order);
    }
}
