using MyWeb.Models;
using MyWeb.ViewModels;

namespace MyWeb.Services.Interfaces
{
    public interface IOrderService
    {
        // Hàm xử lý đặt hàng
        Task<int> PlaceOrderAsync(CheckoutViewModel vm, string userId, string userEmail);

        Task<Order?> GetOrderForUserAsync(int orderId, string userId);
    }
}