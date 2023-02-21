using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoppu.Domain.Entities;

namespace Shoppu.Domain.ViewModels
{
    public class BrowseSideMenuViewModel
    {
        public string CurrentProductCategoryUrl { get; set; }
        public string CurrentGenderSelected { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public ManageProductsFiltersViewModel? ManageProductsFilters { get; set; }
    }
}