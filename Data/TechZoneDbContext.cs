
using MyWeb.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MyWeb.Data
{
    public class TechZoneDbContext : IdentityDbContext<ApplicationUser>
    {
        public TechZoneDbContext(DbContextOptions<TechZoneDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<ProductAttribute> ProductAttributes => Set<ProductAttribute>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public DbSet<HomeCategory> HomeCategories => Set<HomeCategory>();

        public DbSet<Banner> Banners => Set<Banner>();

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


        public DbSet<Policy> Policies => Set<Policy>();
        public DbSet<Review> Reviews => Set<Review>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            //chống spam/double click, 1 request oke thì các request sau bị deny
            builder.Entity<Review>()
                .HasIndex(r => new { r.ProductId, r.UserId })
                .IsUnique();

        }

    }
}
