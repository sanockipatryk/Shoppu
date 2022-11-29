using Microsoft.EntityFrameworkCore;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<ProductType> ProductTypes { get; set; }
        DbSet<Variant> Variants { get; set; }
        DbSet<Size> Sizes { get; set; }
        DbSet<ProductVariant> ProductVariants { get; set; }
        DbSet<ProductVariantSize> ProductVariantSizes { get; set; }
        DbSet<Cart> Carts { get; set; }
        DbSet<CartItem> CartItems { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetails> OrderDetails { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
    }
}
