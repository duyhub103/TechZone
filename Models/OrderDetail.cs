using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        // giá tại lúc mua
        public decimal Price { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }

    }
}
