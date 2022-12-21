using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Infrastructure.Persistence
{
    //dotnet ef migrations add AddInitialModels --project ..\Shoppu.Infrastructure\Shoppu.Infrastructure.csproj
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(
                assembly: typeof(ApplicationDbContext).Assembly);

            builder.Entity<ProductCategory>()
                .HasMany(p => p.Sizes)
                .WithOne(s => s.ProductCategory)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Variant> Variants { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<ProductVariant> ProductVariants { get; set; }
        public virtual DbSet<ProductVariantImage> ProductVariantImages { get; set; }
        public virtual DbSet<ProductVariantSize> ProductVariantSizes { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
    }
}