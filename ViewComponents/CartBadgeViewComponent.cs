using Microsoft.AspNetCore.Mvc;
using MyWeb.Repositories.Interfaces;
using System.Security.Claims;

namespace MyWeb.ViewComponents
{
    public class CartBadgeViewComponent : ViewComponent
    {
        private readonly ICartRepository _cartRepo;

        public CartBadgeViewComponent(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 1. Lấy UserId hiện tại
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. Nếu chưa đăng nhập thì Giỏ hàng = 0
            if (userId == null)
            {
                return View(0);
            }

            // 3. Lấy giỏ hàng từ DB
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);

            // 4. Đếm số dòng (Distinct Products)
            // đếm tổng số lượng thì dùng .Sum(x => x.Quantity)
            int count;

            if (cart != null)
            {
                count = cart.CartItems.Count;
            }
            else
            {
                count = 0;
            }

            return View(count);
        }
    }
}
