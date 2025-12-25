namespace MyWeb.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();

        public decimal SubTotal { get; set; }          // Tổng tiền hàng
        public decimal ShippingFee { get; set; }       // Phí ship
        public decimal DiscountAmount { get; set; }    // Giảm giá
        public decimal GrandTotal { get; set; }        // Tổng cuối

        public string ShippingFeeText =>
        ShippingFee == 0 ? "Miễn phí" : ShippingFee.ToString("#,##0₫");
    }
}
