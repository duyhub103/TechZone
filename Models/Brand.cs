using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(300)]
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }

        // Navigation property
        public ICollection<Product>? Products { get; set; }
    }
}
