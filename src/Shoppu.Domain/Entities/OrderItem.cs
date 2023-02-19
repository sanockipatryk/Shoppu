using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(ProductVariantSize))]
        public int ProductVariantSizeId { get; set; }
        public ProductVariantSize ProductVariantSize { get; set; }

        [ForeignKey(nameof(Order))]
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}
