using MyWeb.Models;
using MyWeb.ViewModels;
using System.Text;

namespace MyWeb.Helpers
{
    public static class EmailContent
    {
        public static string GenerateOrderSuccess(Order order, List<CartItemViewModel> items)
        {
            var sb = new StringBuilder();
            sb.Append($"<div style='font-family: sans-serif; padding: 20px;'>");
            sb.Append($"<h1 style='color: #f9f506; background: #181811; padding: 10px;'>TechZone - Đặt hàng thành công</h1>");
            sb.Append($"<p>Xin chào <strong>{order.CustomerName}</strong>,</p>");
            sb.Append($"<p>Đơn hàng <strong>#{order.Id}</strong> của bạn đã được tiếp nhận.</p>");

            sb.Append("<table style='width: 100%; border-collapse: collapse; margin-top: 20px;'>");
            sb.Append("<tr style='background: #f0f0f0;'><th style='padding: 10px; text-align: left;'>Sản phẩm</th><th style='padding: 10px;'>SL</th><th style='padding: 10px; text-align: right;'>Thành tiền</th></tr>");

            foreach (var item in items)
            {
                sb.Append("<tr>");
                sb.Append($"<td style='padding: 10px; border-bottom: 1px solid #ddd;'>{item.ProductName}</td>");
                sb.Append($"<td style='padding: 10px; border-bottom: 1px solid #ddd; text-align: center;'>{item.Quantity}</td>");
                sb.Append($"<td style='padding: 10px; border-bottom: 1px solid #ddd; text-align: right;'>{(item.Price * item.Quantity):N0}đ</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");

            sb.Append($"<h3 style='text-align: right; margin-top: 20px;'>Tổng thanh toán: {order.FinalAmount:N0}đ</h3>");
            sb.Append($"<p>Địa chỉ giao hàng: {order.ReceiverAddress}</p>");
            sb.Append($"<p>Chúng tôi sẽ sớm liên hệ để giao hàng.</p>");
            sb.Append("</div>");

            return sb.ToString();
        }
    }
}