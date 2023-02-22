using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.ViewModels
{
    public class CreateColorViewModel
    {
        [Required(ErrorMessage = "Please enter name for the color.")]
        [MinLength(3, ErrorMessage = "Color name should be at least 3 characters long.")]
        [Display(Name = "Name of color to display")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please choose HEX code for the color.")]
        [MinLength(7, ErrorMessage = "Hex code should be 7 characters long.")]
        [RegularExpression("^#[0-9A-Fa-f]{6}$", ErrorMessage = "Entered value is not a HEX code.")]
        [Display(Name = "Color to display")]
        public string HEXColor { get; set; }
    }
}