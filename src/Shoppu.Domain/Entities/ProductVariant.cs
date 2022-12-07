using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public string Slug { get; set; }


        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [ForeignKey(nameof(Variant))]
        public int VariantId { get; set; }
        public Variant? Variant { get; set; }

        public List<ProductVariantImage>? Images { get; set; }
        public List<ProductVariantSize>? Sizes { get; set; }
    }
}
