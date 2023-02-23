using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.ViewModels
{
    public class CreateProductVariantViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Choose a variant for this product.")]
        [Display(Name = "Select variant color")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Select variant color")]
        public int VariantId { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Name should be between 5 to 50 characters long.")]
        [Display(Name = "Specific name for the variant")]
        public string? Name { get; set; }
        [Display(Name = "Specific price of this variant")]
        public string? Price { get; set; }

        [Required(ErrorMessage = "Add assigned variation code.")]
        [Display(Name = "Variation code")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Variation code should have a total of 2 characters.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Variation code should consist of only letters and numbers.")]
        public string CodeAddition { get; set; }
        public string? ProductCategoryUrl { get; set; }
    }
}
