using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;
using MyWeb.Services.Interfaces;
using MyWeb.ViewModels;

namespace MyWeb.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderRepository _orderRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(
        ICartService cartService,
        IOrderRepository orderRepo,
        UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _orderRepo = orderRepo;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _cartService.GetCartByUserIdAsync(user!.Id);

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
        public async Task<IActionResult> Index(CheckoutViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.GetUserAsync(User);
            var cart = await _cartService.GetCartByUserIdAsync(user!.Id);

            var order = new Order
            {
                UserId = user.Id,
                ReceiverName = vm.ReceiverName,
                ReceiverPhone = vm.ReceiverPhone,
                ReceiverEmail = vm.ReceiverEmail,
                ReceiverAddress = vm.ReceiverAddress,
                Note = vm.Note,
                PaymentMethod = vm.PaymentMethod,
                ShippingFee = cart.ShippingFee,
                DiscountAmount = 0,
                FinalAmount = cart.GrandTotal
            };

            await _orderRepo.CreateOrderAsync(order);
            await _cartService.ClearCartByUserAsync(user.Id);

            return RedirectToAction("OrderSuccess");
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
