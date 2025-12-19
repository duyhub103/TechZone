using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string CustomerName { get; set; } = null!;

        [Required, StringLength(20)]
        public string Phone { get; set; } = null!;

        [Required, StringLength(300)]
        public string Address { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Status { get; set; } = "Pending";
        
        public decimal TotalAmount { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
