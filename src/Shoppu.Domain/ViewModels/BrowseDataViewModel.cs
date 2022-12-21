using Shoppu.Domain.Entities;

namespace Shoppu.Domain.ViewModels
{
    public class BrowseDataViewModel
    {
        public List<BrowseDataProductItemViewModel> Products { get; set; }

        public List<Size> PossibleSizes { get; set; }
        public List<Variant> PossibleVariants { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public BrowseProductsFiltersViewModel? Filters { get; set; }
    }

    public class BrowseDataProductItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int VariantId { get; set; }
        public string? VariantName { get; set; }
        public decimal? VariantPrice { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public List<string> ImagePaths { get; set; }
    }
}

