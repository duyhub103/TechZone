using MyWeb.ViewModels;
using MyWeb.Models;

namespace MyWeb.Services.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts(string? type = null, string? value = null);
        ProductDetailViewModel GetProductDetail(int id);
    }
}
