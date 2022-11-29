using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class ProductVariantSize
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(Variant))]
        public int VariantId { get; set; }
        public Variant Variant { get; set; }

        [ForeignKey(nameof(Size))]
        public int SizeId { get; set; }
        public Size Size { get; set; }
    }
}
