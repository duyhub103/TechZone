using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models
{
    public enum BannerPosition
    {
        MainSlider = 0,
        PromoSection = 1
    }

    public class Banner
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Subtitle { get; set; }

        [Required, MaxLength(300)]
        public string ImageUrl { get; set; } = string.Empty; // ex: "/images/home/banners/hero-1.jpg"

        [MaxLength(300)]
        public string? LinkUrl { get; set; } // ex: "/Product"

        [Required]
        public BannerPosition Position { get; set; } = BannerPosition.MainSlider;

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
