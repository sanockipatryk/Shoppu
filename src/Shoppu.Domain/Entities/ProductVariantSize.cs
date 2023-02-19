using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class ProductVariantSize
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(ProductVariant))]
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        [ForeignKey(nameof(Size))]
        public int SizeId { get; set; }
        public Size Size { get; set; }

        [NotMapped]
        public int? QuantitySold { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
}
