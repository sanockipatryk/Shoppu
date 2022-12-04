using System.ComponentModel.DataAnnotations;
using Shoppu.Domain.Validations;

namespace Shoppu.Domain.ViewModels
{
    public class CreateProductVariantSizeViewModel
    {
        [Display(Name = "Add size")]
        public bool AddSizeAsVariantOption { get; set; } = false;
        [RequiredIfTrue(nameof(AddSizeAsVariantOption), ErrorMessage = "Enter quantity number.")]
        [Display(Name = "Available quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must not be negative.")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int? Quantity { get; set; }
        [Required]
        public int SizeId { get; set; }
        public string? SizeName { get; set; }
    }
}
