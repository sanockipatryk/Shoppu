﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlName { get; set; }

        public int? ParentCategoryId { get; set; }
        public ProductCategory? ParentCategory { get; set; }

        public List<ProductCategory>? SubCategories { get; set; }
        public List<Product>? Products { get; set; }

        [NotMapped]
        public bool HasNoExistingProducts { get; set; }
    }
}
