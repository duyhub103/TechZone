using MyWeb.Models;

namespace MyWeb.Repositories.Interfaces
{
    public interface IBannerRepository
    {
        IEnumerable<Banner> GetMainSliders();
        Banner? GetPromoBanner();
    }
}
