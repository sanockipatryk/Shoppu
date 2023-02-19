using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppu.Domain.ViewModels
{
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
        public DateTime? DateSetAsAccessible { get; set; }
    }
}