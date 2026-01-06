using MyWeb.Models;

namespace MyWeb.Repositories.Interfaces
{
    public interface IBannerRepository
    {
        Task<List<Banner>> GetMainSlidersAsync();
        Task<Banner?> GetPromoBannerAsync();
    }
}
