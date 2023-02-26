using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.ViewModels
{
    public class EditProductCategoryViewModel
    {
        [Required(ErrorMessage = "Select the product category to edit.")]
        public int? CategoryId { get; set; }
        [MinLength(3, ErrorMessage = "Category name should be at least 3 letters long.")]
        [Display(Name = "Updated category name")]
        public string? Name { get; set; }
        [MinLength(3, ErrorMessage = "Category Url name should be at least 3 letters long.")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Category Url name should be a single word that uses only letters.")]
        [Display(Name = "Updated Category Url name")]
        public string? UrlName { get; set; }

        [Display(Name = "Gender target of Category")]
        public string? SpecificGender { get; set; }
    }
}