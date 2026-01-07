using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Repositories.Interfaces;
using MyWeb.ViewModels;

namespace MyWeb.Services.Implementations
{
    public class HomeService 
    {
        private readonly IProductRepository _productRepo;
        private readonly IBannerRepository _bannerRepo;
        private readonly TechZoneDbContext _context;

        public HomeService(
            IProductRepository productRepo,
            IBannerRepository bannerRepo,
            TechZoneDbContext context)
        {
            _productRepo = productRepo;
            _bannerRepo = bannerRepo;
            _context = context;
        }

        public async Task<HomeViewModel> GetHomeViewModelAsync()
        {
            return new HomeViewModel
            {
                FeaturedProducts = await _productRepo.GetFeaturedAsync(4),
                MainSliders = await _bannerRepo.GetMainSlidersAsync(),
                PromoBanner = await _bannerRepo.GetPromoBannerAsync(),
                PopularCategories = await _context.HomeCategories
                                    .OrderBy(x => x.DisplayOrder)
                                    .ToListAsync()
            };
        }
    }
}
