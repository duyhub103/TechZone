using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required, StringLength(300)]
        public string Url { get; set; } = null!;

        public Product? Product { get; set; }
    }
}
