using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.Enums
{
    public enum ProductSortBy
    {
        [Display(Name = "From A to Z")]
        AZ = 0,
        [Display(Name = "From Z to A")]
        ZA = 1,
        [Display(Name = "Price - from lowest")]
        PriceLowest = 2,
        [Display(Name = "Price - from highest")]
        PriceHighest = 3,
        [Display(Name = "Newest first")]
        MostRecent = 4,
        [Display(Name = "Oldest first")]
        LeastRecent = 5
    }
}