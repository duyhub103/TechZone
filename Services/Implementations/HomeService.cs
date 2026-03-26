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

        public HomeService(
            IProductRepository productRepo,
            IBannerRepository bannerRepo)
        {
            _productRepo = productRepo;
            _bannerRepo = bannerRepo;
        }

        public async Task<HomeViewModel> GetHomeViewModelAsync()
        {
            return new HomeViewModel
            {
                FeaturedProducts = await _productRepo.GetFeaturedAsync(4),
                LatestProducts = await _productRepo.GetLatestAsync(4),
                BestSellingProducts = await _productRepo.GetBestSellingAsync(4),
                MainSliders = await _bannerRepo.GetMainSlidersAsync(),
                PromoBanner = await _bannerRepo.GetPromoBannerAsync(),
                PopularCategories = await _bannerRepo.GetPopularCategoriesAsync()
            };
        }
    }
}
