using Shoppu.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; }
        [StringLength(1000, MinimumLength = 5)]
        public string Description { get; set; }
        public ProductGender Gender { get; set; }

        [Column(TypeName = "decimal(14,2)")]
        public decimal Price { get; set; }
        public bool IsAccessible { get; set; }
        public string BaseSlug { get; set; }
        public string Code { get; set; }

        [ForeignKey(nameof(ProductCategory))]
        public int ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; }

        public List<ProductVariant>? Variants { get; set; }

    }
}
