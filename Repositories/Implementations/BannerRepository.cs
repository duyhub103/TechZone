using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Banner>> GetMainSlidersAsync()
        {
            return await _context.Banners
                .Where(b => b.IsActive && b.Position == BannerPosition.MainSlider)
                .OrderBy(b => b.DisplayOrder)
                .ToListAsync();
        }

        public async Task<Banner?> GetPromoBannerAsync()
        {
            return await _context.Banners
                .Where(b => b.IsActive && b.Position == BannerPosition.PromoSection)
                .OrderBy(b => b.DisplayOrder)
                .FirstOrDefaultAsync();
        }
    }
}
