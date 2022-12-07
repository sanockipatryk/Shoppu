using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppu.Domain.ViewModels
{
    public class BrowseProductViewModel
    {
        public List<BrowseProductItemViewModel> Products { get; set; }
    }

    public class BrowseProductItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int VariantId { get; set; }
        public string VariantName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Slug { get; set; }
        public decimal Price { get; set; }
        public List<string> ImagePaths { get; set; }

    }
}

