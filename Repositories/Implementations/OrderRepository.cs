using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;
using MyWeb.ViewModels;
namespace MyWeb.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly TechZoneDbContext _context;

        public OrderRepository(TechZoneDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasUserPurchasedProductAsync(string userId, int productId)
        {
            return await _context.Orders
            .AnyAsync(o =>
                o.UserId == userId &&
                o.Status == "Success" &&
                o.OrderDetails!.Any(od => od.ProductId == productId)
            );
        }

        //public async Task<int> CreateOrderAsync(Order order)
        //{
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();
        //    return order.Id;
        //}

        public async Task<int> CreateOrderWithTransactionAsync(Order order, List<CartItemViewModel> cartItems)
        {
            // Begin Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //  Trừ kho (Atomic Update)
                foreach (var item in cartItems)
                {
                    // check stock khi update
                    string sql = "UPDATE Products SET Stock = Stock - {0}, Sold = Sold + {0} WHERE Id = {1} AND Stock >= {0}";
                    int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, item.Quantity, item.ProductId);

                    if (rowsAffected == 0)
                    {
                        // Nếu trả về false -> Kho không đủ hoặc sp không tồn tại
                        // Rollback
                        await transaction.RollbackAsync();
                        throw new Exception($"Đã xảy ra lỗi khi đặt sản phẩm '{item.ProductName}'.");
                    }
                }

                //  Lưu Order Header
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                //  Lưu Order Details
                var details = cartItems.Select(item => new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList();

                _context.OrderDetails.AddRange(details);
                await _context.SaveChangesAsync();

                //  Commit Transaction
                await transaction.CommitAsync();

                return order.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; // Ném lỗi lên Service
            }
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)!
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
