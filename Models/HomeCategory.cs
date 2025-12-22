using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models
{
    public class HomeCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Icon { get; set; } = null!;

        public HomeCategoryLinkType LinkType { get; set; }

        // Giá trị để filter (CategoryId | BrandName | Attribute)
        [Required]
        public string LinkValue { get; set; } = null!;

        public int DisplayOrder { get; set; }
    }

    public enum HomeCategoryLinkType
    {
        Category = 0,
        Brand = 1,
        Attribute = 2
    }
}
