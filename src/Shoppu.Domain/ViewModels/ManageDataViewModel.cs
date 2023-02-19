using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoppu.Domain.Entities;

namespace Shoppu.Domain.ViewModels
{
    public class ManageDataViewModel
    {
        public List<Product> Products { get; set; }
        public ManageProductsFiltersViewModel? AppliedFilters { get; set; }
        public PaginationViewModel? Pagination { get; set; }

    }
}