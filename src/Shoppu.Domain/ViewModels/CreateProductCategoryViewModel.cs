using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.ViewModels
{
    public class CreateProductCategoryViewModel
    {
        [Required(ErrorMessage = "Select the product category.")]
        public int? ParentCategoryId { get; set; }
        [Required(ErrorMessage = "Please enter Category name")]
        [MinLength(3, ErrorMessage = "Category name should be at least 3 letters long.")]
        [Display(Name = "New category name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter Category Url name")]
        [MinLength(3, ErrorMessage = "Category Url name should be at least 3 letters long.")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Category Url name should be a single word that uses only letters.")]
        [Display(Name = "New category Url name")]
        public string UrlName { get; set; }

        [Display(Name = "Gender target of Category")]
        public string? SpecificGender { get; set; }
    }
}