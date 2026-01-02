using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // thông tin tài chính
        public decimal ShippingFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public decimal TotalAmount { get; set; }
        

        // thông tin người nhận (nếu đặt dùm)
        [StringLength(150)]
        public string ReceiverName { get; set; } = null!;

        [StringLength(20)]
        public string ReceiverPhone { get; set; } = null!;

        [StringLength(300)]
        public string ReceiverAddress { get; set; } = null!;

        [StringLength(150)]
        public string? ReceiverEmail { get; set; }

        [StringLength(500)]
        public string? Note { get; set; }

        // Phương thức thanh toán
        public string PaymentMethod { get; set; } = "COD"; // Mặc định COD


        public ICollection<OrderDetail>? OrderDetails { get; set; }

        // Liên kết với User
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
