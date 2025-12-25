using MyWeb.Data;
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;
namespace MyWeb.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TechZoneDbContext _context;

        public OrderRepository(TechZoneDbContext context)
        {
            _context = context;
        }

        public bool HasUserPurchasedProduct(string userId, int productId)
        {
            return _context.Orders
            .Any(o =>
                o.UserId == userId &&
                o.Status == "Success" &&
                o.OrderDetails!.Any(od => od.ProductId == productId)
            );
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }
    }
}
