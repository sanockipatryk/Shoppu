using System.ComponentModel.DataAnnotations;
using Shoppu.Domain.Enums;

namespace Shoppu.Domain.ViewModels
{
    public class ManageProductsFiltersViewModel
    {
        [Display(Name = "Product name")]
        public string? Name { get; set; }
        [Display(Name = "Targeted gender")]
        public ProductGender? Gender { get; set; }
        [Display(Name = "Accessibility status")]
        public ProductAccessibilityStatus? AccessibilityStatus { get; set; }
        [Display(Name = "Products without any variants")]
        public bool WithoutAnyVariants { get; set; } = false;
        [Display(Name = "Variants without specified sizes")]
        public bool WithoutSpecifiedSizes { get; set; } = false;
    }
}