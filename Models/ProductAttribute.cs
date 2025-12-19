using MyWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models
{
    public class ProductAttribute
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required, StringLength(100)]
        public string Key { get; set; } = null!;

        [Required, StringLength(200)]
        public string Value { get; set; } = null!;


        public Product? Product { get; set; }
    }
}
