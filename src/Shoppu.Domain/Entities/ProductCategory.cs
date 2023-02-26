using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
        [NotMapped]
        public string QueryRoute
        {
            get
            {
                if (ParentCategory != null)
                {

                    StringBuilder stringBuilder = new StringBuilder("");

                    AppendQueryRoute(stringBuilder, this, 0);

                    void AppendQueryRoute(StringBuilder stringBuilder, ProductCategory category, int level)
                    {
                        if (category.ParentCategory != null)
                        {
                            if (level == 0)
                            {
                                stringBuilder.Append(category.ParentCategory.UrlName);
                                level++;
                            }
                            else
                            {
                                stringBuilder.Insert(0, "-");
                                stringBuilder.Insert(0, category.ParentCategory.UrlName);
                                level++;
							}

							AppendQueryRoute(stringBuilder, category.ParentCategory, level);
                        }
                    }

                    return stringBuilder.ToString();

                }
                return "all";
            }
        }

        public string? SpecificGender { get; set; }
    }
}
