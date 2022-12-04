using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class ProductVariantImage
    {
        public int Id { get; set; }
        public string ImageSource { get; set; }
        [ForeignKey(nameof(ProductVariant))]
        public int ProductVariantId { get; set; }
        public ProductVariant? ProductVariant { get; set; }
    }
}
