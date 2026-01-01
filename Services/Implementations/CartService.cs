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
        private readonly IProductRepository _productRepo;

        public CartService(ICartRepository cartRepo, IProductRepository productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
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

            var product = _productRepo.GetById(productId);

            int currentQtyItem = 0;

            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            if (product.Stock <= 0)
            {
                throw new Exception("Liên hệ với chúng tôi qua hotline hoặc Email để nhận tư vấn về sản phẩm!");
            }

            if (cart == null)
            {
                cart = await _cartRepo.CreateCartAsync(userId);
            }
            else
            {
                //check sản phẩm tồn tại trong cart chưa
                var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
                if (existingItem != null)
                {
                    currentQtyItem = existingItem.Quantity;
                }
            }

            int newQtyItem = currentQtyItem + quantity;

            if (newQtyItem > product.Stock)
            {
                throw new Exception($"Số lượng yêu cầu vượt quá hạn mức.");
            }

            await _cartRepo.AddToCartAsync(cart.Id, productId, quantity);
        }

        public async Task RemoveItemAsync(string userId, int productId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                await _cartRepo.RemoveItemAsync(cart.Id, productId);
            }
        }


        public async Task ClearCartByUserAsync(string userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart != null)
                await _cartRepo.ClearCartAsync(cart.Id);
        }

        public async Task UpdateQuantityAsync(string userId, int productId, int quantity)
        {
            var product = _productRepo.GetById(productId); //check stock
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            if (quantity > product.Stock)
            {
                throw new Exception($"Số lượng yêu cầu vượt quá hạn mức.");
            }

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                await _cartRepo.UpdateQuantityAsync(cart.Id, productId, quantity);
            }
        }
    }
}
