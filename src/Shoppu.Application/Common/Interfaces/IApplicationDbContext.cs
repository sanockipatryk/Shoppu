using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(params object[] entities);
        EntityEntry Update(object entity);
        void UpdateRange(IEnumerable<object> entities);

        DbSet<Product> Products { get; set; }
        DbSet<ProductCategory> ProductCategories { get; set; }
        DbSet<Variant> Variants { get; set; }
        DbSet<Size> Sizes { get; set; }
        DbSet<ProductVariant> ProductVariants { get; set; }
        DbSet<ProductVariantImage> ProductVariantImages { get; set; }
        DbSet<ProductVariantSize> ProductVariantSizes { get; set; }
        DbSet<Cart> Carts { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetails> OrderDetails { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
    }
}
