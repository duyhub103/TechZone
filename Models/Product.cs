using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace MyWeb.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        public int Stock { get; set; }
        public int Sold { get; set; } = 0;

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        // foreign key relationship
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int BrandId { get; set; }
        public Brand? Brand { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // navigation properties
        public ICollection<ProductAttribute>? Attributes { get; set; }
        public ICollection<ProductImage>? Images { get; set; }
    }
}
