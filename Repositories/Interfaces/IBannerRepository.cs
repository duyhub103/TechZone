using MyWeb.Models;

namespace MyWeb.Repositories.Interfaces
{
    public interface IBannerRepository 
        //IhomeRepo
    {
        Task<List<Banner>> GetMainSlidersAsync();
        Task<Banner?> GetPromoBannerAsync();

        Task<List<HomeCategory>> GetPopularCategoriesAsync();
    }
}
