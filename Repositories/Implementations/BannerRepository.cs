using MyWeb.Data;
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;

namespace MyWeb.Repositories.Implementations
{
    public class BannerRepository : IBannerRepository
    {
        private readonly TechZoneDbContext _context;

        public BannerRepository(TechZoneDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Banner> GetMainSliders()
        {
            return _context.Banners
                .Where(b => b.IsActive && b.Position == BannerPosition.MainSlider)
                .OrderBy(b => b.DisplayOrder)
                .ToList();
        }

        public Banner? GetPromoBanner()
        {
            return _context.Banners
                .Where(b => b.IsActive && b.Position == BannerPosition.PromoSection)
                .OrderBy(b => b.DisplayOrder)
                .FirstOrDefault();
        }
    }
}
