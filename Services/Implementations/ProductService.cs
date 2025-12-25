using MyWeb.Models;
using MyWeb.Repositories.Interfaces;
using MyWeb.Services.Interfaces;
using MyWeb.ViewModels;

namespace MyWeb.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        

        public ProductService(
            IProductRepository productRepo
            )
        {
            _productRepo = productRepo;
            
        }

        public IEnumerable<Product> GetAllProducts(string? type = null, string? value = null)
        {
            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
            {
                return _productRepo.GetByFilter(type, value);
            }
            return _productRepo.GetAllActive();
        }

        public ProductDetailViewModel GetProductDetail(int id)
        {
            var mainProduct = _productRepo.GetById(id);
            if (mainProduct == null)
                throw new Exception("Product not found");

            var relatedProducts = _productRepo.GetRelatedProducts(
                mainProduct.CategoryId,
                id,
                4
            );

            return new ProductDetailViewModel
            {
                MainProduct = mainProduct,
                RelatedProducts = relatedProducts.ToList()
            };
        }


    }
}