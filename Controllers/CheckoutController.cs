using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;
using MyWeb.Services;
using MyWeb.ViewModels;

namespace MyWeb.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CartService _cartService;
        private readonly OrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(
            CartService cartService,
            OrderService orderService,
            UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _orderService = orderService;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _cartService.GetCartByUserIdAsync(user!.Id);

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var vm = new CheckoutViewModel
            {
                ReceiverName = user.FullName,
                ReceiverPhone = user.PhoneNumber!,
                ReceiverEmail = user.Email!,
                CartItems = cart.Items,
                SubTotal = cart.SubTotal,
                ShippingFee = cart.ShippingFee,
                GrandTotal = cart.GrandTotal
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel vm)
        {

            var user = await _userManager.GetUserAsync(User);
            var cart = await _cartService.GetCartByUserIdAsync(user!.Id);

            // Validate Form
            if (!ModelState.IsValid)
            {
                // Nếu form lỗi, load lại giỏ hàng để hiện View
                await RepopulateCartData(vm, user.Id);
                return View("Index", vm);
            }

            try
            {
                //  Gọi Service xử lý trọn gói (Transaction: Trừ kho -> Lưu đơn -> Xóa giỏ -> Gửi mail)
                // Truyền user.Email vào để gửi mail xác nhận về chính chủ tài khoản
                int orderId = await _orderService.PlaceOrderAsync(vm, user.Id, user.Email!);
                return RedirectToAction("OrderSuccess", new { id = orderId });
            }
            catch (Exception ex)
            {
                // Bắt lỗi Ví dụ: Sản phẩm hết hàng
                ModelState.AddModelError("", ex.Message);

                //Load lại giỏ hàng để check
                await RepopulateCartData(vm, user.Id);
                return View("Index", vm);
            }
        }

        public async Task<IActionResult> OrderSuccess(int id)
        {
            var userId = _userManager.GetUserId(User);

            var order = await _orderService.GetOrderForUserAsync(id, userId!);

            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        // load lại giỏ hàng khi có lỗi
        private async Task RepopulateCartData(CheckoutViewModel vm, string userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                vm.CartItems = cart.Items;
                vm.SubTotal = cart.SubTotal;
                vm.ShippingFee = cart.ShippingFee;
                vm.GrandTotal = cart.GrandTotal;
            }
        }


    }
}
