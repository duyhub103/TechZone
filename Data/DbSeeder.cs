using MyWeb.Models;

namespace MyWeb.Data
{
    public static class DbSeeder
    {
        public static void Seed(TechZoneDbContext context)
        {
            // Seed Products/Category/Brand nếu chưa có
            if (!context.Categories.Any() && !context.Brands.Any() && !context.Products.Any())
            {
                var catLaptop = new Category { Name = "Laptop", Description = "Laptop các phân khúc" };
                var catAccessory = new Category { Name = "Phụ kiện", Description = "Phụ kiện công nghệ" };
                context.Categories.AddRange(catLaptop, catAccessory);
                context.SaveChanges();

                var bApple = new Brand { Name = "Apple", ImageUrl = "/images/brands/apple.png" };
                var bAsus = new Brand { Name = "Asus", ImageUrl = "/images/brands/asus.png" };
                var bDell = new Brand { Name = "Dell", ImageUrl = "/images/brands/dell.png" };
                var bLenovo = new Brand { Name = "Lenovo", ImageUrl = "/images/brands/lenovo.png" };
                var bLogitech = new Brand { Name = "Logitech", ImageUrl = "/images/brands/logitech.png" };
                context.Brands.AddRange(bApple, bAsus, bDell, bLenovo, bLogitech);
                context.SaveChanges();

                Product NewProduct(string name, decimal price, int catId, int brandId, string imageUrl, bool featured = false, string? desc = null)
                    => new Product
                    {
                        Name = name,
                        Price = price,
                        CategoryId = catId,
                        BrandId = brandId,
                        ImageUrl = imageUrl,
                        IsFeatured = featured,
                        IsActive = true,
                        Description = desc ?? $"<p><strong>{name}</strong> - Sản phẩm demo để test UI & lọc nâng cao.</p>"
                    };

                var p1 = NewProduct("Asus ROG Zephyrus G14", 32990000, catLaptop.Id, bAsus.Id, "/images/products/asus-rog-zephyrus-g14.jpg", featured: true);
                var p2 = NewProduct("Dell Alienware m15", 45000000, catLaptop.Id, bDell.Id, "/images/products/dell-alienware-m15.jpg", featured: true);
                var p3 = NewProduct("MacBook Air M2", 26990000, catLaptop.Id, bApple.Id, "/images/products/macbook-air-m2.jpg", featured: true);
                var p4 = NewProduct("Dell XPS 13 Plus", 41990000, catLaptop.Id, bDell.Id, "/images/products/dell-xps-13-plus.jpg");
                var p5 = NewProduct("Lenovo ThinkPad X1", 38990000, catLaptop.Id, bLenovo.Id, "/images/products/lenovo-thinkpad-x1.jpg");
                var p6 = NewProduct("Chuột Logitech MX Master 3S", 2500000, catAccessory.Id, bLogitech.Id, "/images/products/logitech-mx-master-3s.jpg");
                var p7 = NewProduct("Bàn phím cơ Asus ROG Azoth", 6000000, catAccessory.Id, bAsus.Id, "/images/products/asus-rog-azoth.jpg");
                var p8 = NewProduct("Lenovo Legion 5", 29990000, catLaptop.Id, bLenovo.Id, "/images/products/lenovo-legion-5.jpg");
                var p9 = NewProduct("Dell Inspiron 14", 18990000, catLaptop.Id, bDell.Id, "/images/products/dell-inspiron-14.jpg");
                var p10 = NewProduct("Tai nghe Logitech G Pro X", 2990000, catAccessory.Id, bLogitech.Id, "/images/products/logitech-g-pro-x.jpg");

                context.Products.AddRange(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
                context.SaveChanges();

                void AddAttr(Product p, params (string Key, string Value)[] attrs)
                {
                    foreach (var a in attrs)
                    {
                        context.ProductAttributes.Add(new ProductAttribute
                        {
                            ProductId = p.Id,
                            Key = a.Key,
                            Value = a.Value
                        });
                    }
                }

                AddAttr(p1, ("CPU", "Ryzen 9"), ("RAM", "16GB"), ("VGA", "RTX 4060"), ("Nhu cầu", "Gaming"), ("Màn hình", "14 inch 120Hz"));
                AddAttr(p2, ("CPU", "Core i9"), ("RAM", "32GB"), ("Nhu cầu", "Gaming"));
                AddAttr(p3, ("CPU", "Apple M2"), ("RAM", "8GB"), ("Nhu cầu", "Mỏng nhẹ"), ("Màn hình", "Retina"));
                AddAttr(p4, ("CPU", "Core i7"), ("Nhu cầu", "Doanh nhân"), ("Màn hình", "13.4 inch OLED"));
                AddAttr(p5, ("CPU", "Core i7"), ("Nhu cầu", "Văn phòng"), ("Độ bền", "Chuẩn quân đội MIL-STD"));
                AddAttr(p6, ("Loại", "Chuột không dây"), ("DPI", "8000"), ("Nhu cầu", "Văn phòng"));
                AddAttr(p7, ("Loại", "Bàn phím cơ"), ("Switch", "NX Red"), ("Nhu cầu", "Gaming"));
                AddAttr(p8, ("CPU", "Ryzen 7"), ("RAM", "16GB"), ("VGA", "RTX 4060"), ("Nhu cầu", "Gaming"));
                AddAttr(p9, ("CPU", "Core i5"), ("RAM", "16GB"), ("Nhu cầu", "Văn phòng"));
                AddAttr(p10, ("Loại", "Tai nghe gaming"), ("Mic", "Blue VO!CE"), ("Nhu cầu", "Gaming"));

                context.SaveChanges();
            }

            // Seed Banners nếu chưa có
            if (!context.Banners.Any())
            {
                var banners = new List<Banner>
                {
                    new Banner
                    {
                        Title = "MacBook Air M2\nSiêu Mỏng, Siêu Mạnh",
                        Subtitle = "Giảm ngay 15% cho các dòng Apple M2. Tặng kèm quà khi mua online.",
                        ImageUrl = "/images/home/banners/hero-1.jpg",
                        LinkUrl = "/Product",
                        Position = BannerPosition.MainSlider,
                        DisplayOrder = 1,
                        IsActive = true
                    },
                    new Banner
                    {
                        Title = "Laptop Gaming RTX 40 Series\nChiến Game Đỉnh Cao",
                        Subtitle = "Trải nghiệm hiệu năng tối đa với GPU RTX 40 Series mới nhất.",
                        ImageUrl = "/images/home/banners/hero-2.jpg",
                        LinkUrl = "/Product",
                        Position = BannerPosition.MainSlider,
                        DisplayOrder = 2,
                        IsActive = true
                    },
                    new Banner
                    {
                        Title = "Setup Văn Phòng\nMỏng Nhẹ – Pin Trâu",
                        Subtitle = "Gợi ý ultrabook & phụ kiện tối ưu cho công việc mỗi ngày.",
                        ImageUrl = "/images/home/banners/hero-3.jpg",
                        LinkUrl = "/Product",
                        Position = BannerPosition.MainSlider,
                        DisplayOrder = 3,
                        IsActive = true
                    },

                    new Banner
                    {
                        Title = "Chiến Game Đỉnh Cao\nKhông Giới Hạn",
                        Subtitle = "Trải nghiệm sức mạnh đồ họa RTX 40 Series mới nhất trên Laptop Gaming cao cấp.",
                        ImageUrl = "/images/home/promos/promo-1.jpg",
                        LinkUrl = "/Product",
                        Position = BannerPosition.PromoSection,
                        DisplayOrder = 1,
                        IsActive = true
                    }
                };

                context.Banners.AddRange(banners);
                context.SaveChanges();
            }

            // =======================
            // HOME CATEGORIES (Trang chủ)
            // =======================

            if (!context.HomeCategories.Any())
            {
                var homeCategories = new List<HomeCategory>
                {
                    new HomeCategory
                    {
                        Name = "Laptop Gaming",
                        Icon = "sports_esports",
                        LinkType = HomeCategoryLinkType.Attribute,
                        LinkValue = "Gaming",
                        DisplayOrder = 1
                    },
                    new HomeCategory
                    {
                        Name = "Văn Phòng",
                        Icon = "business_center",
                        LinkType = HomeCategoryLinkType.Attribute,
                        LinkValue = "Văn phòng",
                        DisplayOrder = 2
                    },
                    new HomeCategory
                    {
                        Name = "Đồ Họa",
                        Icon = "brush",
                        LinkType = HomeCategoryLinkType.Attribute,
                        LinkValue = "Đồ họa",
                        DisplayOrder = 3
                    },
                    new HomeCategory
                    {
                        Name = "MacBook",
                        Icon = "laptop_mac",
                        LinkType = HomeCategoryLinkType.Brand,
                        LinkValue = "Apple",
                        DisplayOrder = 4
                    },
                    new HomeCategory
                    {
                        Name = "Phụ Kiện",
                        Icon = "headphones",
                        LinkType = HomeCategoryLinkType.Category,
                        LinkValue = "Phụ kiện",
                        DisplayOrder = 5
                    }
                };

                context.HomeCategories.AddRange(homeCategories);
                context.SaveChanges();
            }
        }
    }
}
