using MyWeb.Data;
using MyWeb.Repositories.Interfaces;
using MyWeb.Services.Interfaces;
using MyWeb.ViewModels;

namespace MyWeb.Services.Implementations
{
    public class HomeService : IHomeService
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

        public HomeViewModel GetHomeViewModel()
        {
            return new HomeViewModel
            {
                FeaturedProducts = _productRepo.GetFeatured(4),
                MainSliders = _bannerRepo.GetMainSliders(),
                PromoBanner = _bannerRepo.GetPromoBanner(),
                PopularCategories = _context.HomeCategories
                    .OrderBy(x => x.DisplayOrder)
                    .ToList()
            };
        }
    }
}
