using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.ViewModels
{
    public class CreateProductVariantViewModel
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Choose a variant for this product.")]
        public int VariantId { get; set; }
    }
}
