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

        public PaginationViewModel Pagination { get; set; }
    }
}

