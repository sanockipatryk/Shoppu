using Shoppu.Domain.Entities;
using Shoppu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppu.Domain.ViewModels
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "Add a name for the product.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Name should be between 5 to 50 characters long.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Add a description for the product.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Description should be between 5 to 150 characters long.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Select a gender that the product targets.")]
        public ProductGender? Gender { get; set; }

        [Required(ErrorMessage = "Add a price for the product.")]
        public string Price { get; set; }
        public bool IsShown { get; set; } = false;

        [Required(ErrorMessage = "Select the product category.")]
        public int? ProductCategoryId { get; set; }
    }
}
