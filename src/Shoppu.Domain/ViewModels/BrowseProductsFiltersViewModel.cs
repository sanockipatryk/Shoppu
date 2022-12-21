using Shoppu.Domain.Enums;

namespace Shoppu.Domain.ViewModels
{
    public class BrowseProductsFiltersViewModel
    {
        public ProductSortBy? SortBy { get; set; } = ProductSortBy.AZ;

        public string? MinPrice { get; set; }
        public string? MaxPrice { get; set; }

        public List<int> Sizes { get; set; }
        public List<int> Variants { get; set; }

    }
}