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

        public async Task<PaginatedList<Product>> GetProductsAsync(string? keyword, string? type, string? value, int pageIndex, int pageSize)
        {
            // Khởi tạo query cơ bản
            var query = _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Attributes)
                .AsQueryable();

            // Xử lý Tìm kiếm (Search)
            if (!string.IsNullOrEmpty(keyword))
            {
                // Tìm trong Tên, Danh mục, Thương hiệu hoặc Attribute
                query = query.Where(p =>
                    p.Name.Contains(keyword) ||
                    p.Category.Name.Contains(keyword) ||
                    p.Brand.Name.Contains(keyword) ||
                    p.Attributes.Any(a => a.Value.Contains(keyword))
                );
            }

            // Xử lý Lọc (Filter theo Type/Value từ Sidebar)
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

            // Sắp xếp (Mặc định mới nhất lên đầu)
            query = query.OrderByDescending(p => p.CreatedAt);

            // Trả về PaginatedList (Nó sẽ tự chạy Count và Skip/Take)
            return await PaginatedList<Product>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Attributes)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        }

        public async Task<List<Product>> GetFeaturedAsync(int take)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive && p.IsFeatured)
                .Include(p => p.Category)
                .Include(p => p.Attributes)
                .OrderByDescending(p => p.Id)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<Product>> GetLatestAsync(int take)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Attributes)
                .OrderByDescending(p => p.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<Product>> GetBestSellingAsync(int take)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Attributes)
                .OrderByDescending(p => p.Sold)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<Product>> GetRelatedProductsAsync(int categoryId, int excludeProductId, int take)
        {
            return await _context.Products
                .Where(p =>
                    p.IsActive &&
                    p.CategoryId == categoryId &&
                    p.Id != excludeProductId)
                .Include(p => p.Attributes)
                .OrderByDescending(p => p.IsFeatured)
                .Take(take)
                .ToListAsync();
        }

        //popup gợi ý khi gõ từ khóa
        public async Task<IEnumerable<Product>> SearchLiveAsync(string keyword)
        {
            return await _context.Products
                .Include(p => p.Category) // Include để lấy ảnh/giá
                .Where(p => p.IsActive && (
                    p.Name.Contains(keyword) ||
                    p.Category.Name.Contains(keyword) ||
                    p.Attributes.Any(a => a.Value.Contains(keyword))
                ))
                .OrderByDescending(p => p.Name.StartsWith(keyword)) // Ưu tiên khớp đầu tên
                .Take(5) // Chỉ lấy 5 sản phẩm
                .ToListAsync();
        }

    }
}
