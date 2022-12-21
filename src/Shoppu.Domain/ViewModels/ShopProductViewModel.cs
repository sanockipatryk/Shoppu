using System.ComponentModel.DataAnnotations;
using Shoppu.Domain.Entities;
namespace Shoppu.Domain.ViewModels
{
    public class ShopProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        public int VariantId { get; set; }
        public string VariantName { get; set; }
        public decimal Price { get; set; }
        public decimal? VariantPrice { get; set; }
        public List<string> ImagePaths { get; set; }
        public List<ProductVariantSize> Sizes { get; set; }
        public List<ColorVariantViewModel> ProductVariants { get; set; }
    }
}