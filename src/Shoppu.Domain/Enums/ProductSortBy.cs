using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.Enums
{
    public enum ProductSortBy
    {
        [Display(Name = "Newest first")]
        MostRecent = 0,
        [Display(Name = "Oldest first")]
        LeastRecent = 1,

        [Display(Name = "Price - from lowest")]
        PriceLowest = 2,
        [Display(Name = "Price - from highest")]
        PriceHighest = 3,
        [Display(Name = "From A to Z")]
        AZ = 4,
        [Display(Name = "From Z to A")]
        ZA = 5,
    }
}