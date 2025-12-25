namespace MyWeb.ViewModels
{
    public class CheckoutViewModel
    {
        //  Receiver Info 
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public string ReceiverEmail { get; set; } = null!;
        public string ReceiverAddress { get; set; } = null!;
        public string? Note { get; set; }
        public string PaymentMethod { get; set; } = "COD";

        //  Cart Snapshot 
        public List<CartItemViewModel> CartItems { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GrandTotal { get; set; }

        public string ShippingFeeText =>
        ShippingFee == 0 ? "Miễn phí" : ShippingFee.ToString("#,##0₫");
    }
}
