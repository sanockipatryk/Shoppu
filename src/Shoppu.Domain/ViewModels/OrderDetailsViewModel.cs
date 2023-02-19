using Shoppu.Domain.Entities;
using Shoppu.Domain.SSOT;

namespace Shoppu.Domain.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderDetails OrderDetails { get; set; }
        public decimal ShippingCost { get; set; } = Defaults.DefaultShipmentCost;
        public decimal CartItemsTotalPrice => OrderDetails.Order.Items.Sum(oi => (oi.ProductVariantSize.ProductVariant.Price ?? oi.ProductVariantSize.ProductVariant.Product.Price) * oi.Quantity);
        public decimal CartTotalPrice => CartItemsTotalPrice + ShippingCost;

    }
}