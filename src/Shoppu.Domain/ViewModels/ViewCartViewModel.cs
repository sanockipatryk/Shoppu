using Shoppu.Domain.Entities;
using Shoppu.Domain.SSOT;

namespace Shoppu.Domain.ViewModels
{
    public class ViewCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public NotificationMessageViewModel? CartLoadingNotification { get; set; }
        public decimal ShippingCost { get; set; } = Defaults.DefaultShipmentCost;
        public bool IsEditable { get; set; }
        public decimal CartItemsTotalPrice => CartItems.Sum(ci => (ci.ProductVariantSize.ProductVariant.Price ?? ci.ProductVariantSize.ProductVariant.Product.Price) * ci.Quantity);
        public decimal CartTotalPrice => CartItemsTotalPrice + ShippingCost;
    }
}