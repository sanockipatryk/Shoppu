using Shoppu.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace Shoppu.Domain.ViewModels
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "Add a name for the product.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Name should be between 5 to 50 characters long.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Add a description for the product.")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Description should be between 5 to 500 characters long.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Select a gender that the product targets.")]
        public ProductGender? Gender { get; set; }

        [Required(ErrorMessage = "Add a price for the product.")]
        public string Price { get; set; }
        [Required(ErrorMessage = "Add assigned code of the product.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Code should have a total of 13 characters.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Code should consist of only letters and numbers.")]
        public string Code { get; set; }
        public bool IsAccessible { get; set; } = false;

        [Required(ErrorMessage = "Select the product category.")]
        public int? ProductCategoryId { get; set; }

        public int SizeTypeId { get; set; }
    }
}
