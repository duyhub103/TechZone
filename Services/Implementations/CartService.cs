using MyWeb.Data;
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;
using MyWeb.Services.Interfaces;
using MyWeb.ViewModels;

namespace MyWeb.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;

        public CartService(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task<CartViewModel> GetCartByUserIdAsync(string userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            var vm = new CartViewModel();

            // đảm bảo rằng giỏ hàng không rỗng
            if (cart == null || !cart.CartItems.Any())
                return vm;

            vm.Items = cart.CartItems.Select(i => new CartItemViewModel
            {
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Price = i.Product.Price,
                Quantity = i.Quantity,
                ProductImage = i.Product.ImageUrl
            }).ToList();

            vm.SubTotal = vm.Items.Sum(x => x.Price * x.Quantity);

            const decimal FreeShippingThreshold = 5_000_000m;
            const decimal ShippingFeeAmount = 30_000m;

            if (vm.SubTotal > FreeShippingThreshold)
            {
                vm.ShippingFee = 0;
            }
            else
            {
                vm.ShippingFee = ShippingFeeAmount;
            }

            //vm.ShippingFee = vm.SubTotal > 5_000_000 ? 0 : 30_000;

            vm.DiscountAmount = 0;

            vm.GrandTotal = vm.SubTotal + vm.ShippingFee;

            return vm;
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = await _cartRepo.CreateCartAsync(userId);
            }


            await _cartRepo.AddToCartAsync(cart.Id, productId, quantity);
        }


        public async Task ClearCartByUserAsync(string userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart != null)
                await _cartRepo.ClearCartAsync(cart.Id);
        }
    }
}
