using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppu.Domain.ViewModels
{
    public class ColorVariantViewModel
    {
        public string VariantSlug { get; set; }
        public string ColorName { get; set; }
        public string HEXColor { get; set; }
        public bool IsSelected { get; set; }
    }
}