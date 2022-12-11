using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppu.Domain.ViewModels
{
    public class ManageProductsFiltersViewModel
    {
        public string? Name { get; set; }
        public List<int>? ColorVariants { get; set; }
        public string? Gender { get; set; }
        public int? AccessibleStatus { get; set; }
    }
}