using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;

namespace MyWeb.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly TechZoneDbContext _context;

        public CartRepository(TechZoneDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Cart> CreateCartAsync(string userId)
        {
            var cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);

            await SaveAsync();

            return cart;
        }

        public async Task AddToCartAsync(int cartId, int productId, int quantity)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity,
                    CreatedDate = DateTime.UtcNow
                }; 
                
                _context.CartItems.Add(newItem);
            }

            await SaveAsync();
        }


        public async Task RemoveItemAsync(int cartId, int productId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId);

            if (item != null)
            {
                _context.CartItems.Remove(item);

                await SaveAsync();
            }
        }

        public async Task UpdateQuantityAsync(int cartId, int productId, int newQuantity)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId);

            if (item != null)
            {
                item.Quantity = newQuantity;

                await SaveAsync();
            }
        }

        public async Task ClearCartAsync(int cartId)
        {
            var items = await _context.CartItems.Where(x => x.CartId == cartId).ToListAsync();
            _context.CartItems.RemoveRange(items);

            await SaveAsync();
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
