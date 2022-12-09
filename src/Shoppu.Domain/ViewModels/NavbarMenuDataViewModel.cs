using Shoppu.Domain.Entities;

namespace Shoppu.Domain.ViewModels
{
    public class NavbarMenuDataViewModel
    {
        public List<ProductCategory> MainCategoriesWithTheirSubcategories { get; set; }

        // TODO: 
        // In the future, add support for different categories depending on the gender or age
        // public List<ProductCategory> WomanCategoriesWithTheirSubcategories {get; set;}
        // etc...

    }
}