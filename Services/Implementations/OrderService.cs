using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Helpers;    
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;
using MyWeb.ViewModels;
using System.Text;

namespace MyWeb.Services.Implementations
{
    public class OrderService
    {
        private readonly CartService _cartService;
        private readonly IOrderRepository _orderRepo;
        private readonly EmailSender _emailSender;

        public OrderService(
            CartService cartService,
            IOrderRepository ordertRepo,
            EmailSender emailSender)
        {
            _cartService = cartService;
            _orderRepo = ordertRepo;
            _emailSender = emailSender;
        }

        public async Task<int> PlaceOrderAsync(CheckoutViewModel vm, string userId, string userEmail)
        {
            // Lấy giỏ hàng
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
            {
                throw new Exception("Giỏ hàng trống.");
            }

            // Chuẩn bị dữ liệu Order (Mapping)
            var order = new Order
            {
                UserId = userId,
                CustomerName = vm.ReceiverName ?? "Guest",
                Phone = vm.ReceiverPhone,
                Address = vm.ReceiverAddress,
                ReceiverName = vm.ReceiverName,
                ReceiverPhone = vm.ReceiverPhone,
                ReceiverEmail = vm.ReceiverEmail,
                ReceiverAddress = vm.ReceiverAddress,
                Note = vm.Note,
                PaymentMethod = vm.PaymentMethod,
                Status = "Pending",
                OrderDate = DateTime.Now,
                ShippingFee = cart.ShippingFee,
                DiscountAmount = cart.DiscountAmount,
                TotalAmount = cart.SubTotal,
                FinalAmount = cart.GrandTotal
            };

            //  Gọi Repo để xử lý Transaction (Trừ kho + Lưu đơn)
            int orderId = await _orderRepo.CreateOrderWithTransactionAsync(order, cart.Items);

            // Xóa giỏ hàng khi lưu đơn thành công
            await _cartService.ClearCartByUserAsync(userId);

            try
            {
                // Gửi Email
                string htmlBody = EmailContent.GenerateOrderSuccess(order, cart.Items);
                await _emailSender.SendEmailAsync(userEmail, $"Đơn hàng #{orderId} - TechZone", htmlBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi gửi mail: " + ex.Message);
            }
            

            return orderId;
        }

        public async Task<Order?> GetOrderForUserAsync(int orderId, string userId)
        {
            var order = await _orderRepo.GetOrderByIdAsync(orderId);

            if (order == null || order.UserId != userId.ToString())
            {
                return null;
            }

            return order;
        }

        // chuyển qua helpers
        //private string GenerateEmailBody(Order order, List<CartItemViewModel> items)
        //{
        //    var sb = new StringBuilder();
        //    sb.Append($"<h1>Cảm ơn bạn đã đặt hàng tại TechZone</h1>");
        //    sb.Append($"<p>Mã đơn hàng: <strong>#{order.Id}</strong></p>");
        //    sb.Append($"<p>Tổng thanh toán: <strong>{order.FinalAmount:N0}đ</strong></p>");
        //    sb.Append("<hr/>");
        //    sb.Append("<h3>Chi tiết đơn hàng:</h3><ul>");
        //    foreach (var item in items)
        //    {
        //        sb.Append($"<li>{item.ProductName} x {item.Quantity} : {(item.Price * item.Quantity):N0}đ</li>");
        //    }
        //    sb.Append("</ul>");
        //    sb.Append($"<p>Chúng tôi sẽ sớm liên hệ để giao hàng.</p>");
        //    return sb.ToString();
        //}
    }
}