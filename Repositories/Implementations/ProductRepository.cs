using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWeb.Data;
using MyWeb.Models;
using MyWeb.Repositories.Interfaces;
using MyWeb.ViewModels;

namespace MyWeb.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly TechZoneDbContext _context;

        public ProductRepository(TechZoneDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllActive()
        {
            return _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Attributes)
                .ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Attributes)
                .FirstOrDefault(p => p.Id == id && p.IsActive);

        }

        public IEnumerable<Product> GetFeatured(int take)
        {
            return _context.Products
                .Where(p => p.IsActive && p.IsFeatured)
                .Include(p => p.Category)
                .Include(p => p.Attributes)
                .OrderByDescending(p => p.Id)
                .Take(take)
                .ToList();
        }

        public IEnumerable<Product> GetRelatedProducts(int categoryId, int excludeProductId, int take)
        {
            return _context.Products
                .Where(p =>
                    p.IsActive &&
                    p.CategoryId == categoryId &&
                    p.Id != excludeProductId)
                .Include(p => p.Attributes)
                .OrderByDescending(p => p.IsFeatured)
                .Take(take)
                .ToList();
        }

        public IEnumerable<Product> GetByFilter(string type, string value)
        {
            var query = _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Attributes)
                .AsQueryable();

            switch (type.ToLower())
            {
                case "category":                 
                    query = query.Where(p => p.Category.Name == value);
                    break;
                case "brand":
                    query = query.Where(p => p.Brand.Name == value);
                    break;
                case "attribute":
                    query = query.Where(p => p.Attributes.Any(a => a.Value == value));
                    break;
            }

            return query.ToList();  
        }

        public PaginatedList<Product> GetProducts(string? search, string? type, string? value, int pageIndex, int pageSize)
        {
            // init query, chưa exc
            var query = _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Attributes)
                //.OrderByDescending(p => p.CreateAt)
                .AsQueryable();

            //search
            if (!string.IsNullOrEmpty(search))
            {
                // Tìm theo tên, hãng,, thuộc tính
                query = query.Where(p => p.Name.Contains(search)
                                      || p.Category.Name.Contains(search)
                                      || p.Attributes.Any(a => a.Value.Contains(search))
                                      || p.Brand.Name.Contains(search));
            }


            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
            {
                switch (type.ToLower())
                {
                    case "category":
                        query = query.Where(p => p.Category.Name == value);
                        break;
                    case "brand":
                        query = query.Where(p => p.Brand.Name == value);
                        break;
                    case "attribute":
                        query = query.Where(p => p.Attributes.Any(a => a.Value == value));
                        break;
                }
            }

            //hàm Create của PaginatedList để chạy Skip/Take và Count
            return PaginatedList<Product>.Create(query, pageIndex, pageSize);
        }


        public async Task<LiveSearchViewModel> SearchAsync(string query)
        {
            var model = new LiveSearchViewModel();

            // gợi ý theo keyword
            var suggestionsCategories = await _context.Categories
                .Where(c => c.Name.Contains(query))
                .Select(c => c.Name)
                .Take(3)
                .ToListAsync();

            var suggestionsBrands = await _context.Brands
                .Where(b => b.Name.Contains(query))
                .Select(b => b.Name)
                .Take(3)
                .ToListAsync();

            model.Suggestions.AddRange(suggestionsCategories);
            model.Suggestions.AddRange(suggestionsBrands);

            // tìm sản phẩm theo keyword
            model.Products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive && (
                    p.Name.Contains(query) ||
                    p.Attributes.Any(a => a.Value.Contains(query))
                ))
                .OrderByDescending(p => p.Name.StartsWith(query)) // Ưu tiên khớp đầu từ
                .Take(5)
                .ToListAsync();

            return model;
        }




        //public async Task<bool> ReduceStockAndIncreaseSoldAsync(int productId, int quantity)
        //{
        //    // check stock khi update
        //    string sql = "UPDATE Product SET Stock = Stock - {0}, Sold = Sold + {0} WHERE Id = {1} AND Stock >= {0}";

        //    int result = await _context.Database.ExecuteSqlRawAsync(sql, quantity, productId);

        //    return result > 0;
        //}  chuyển qua order repo
    }
}
