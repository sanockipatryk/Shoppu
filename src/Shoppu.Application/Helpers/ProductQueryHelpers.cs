using Shoppu.Domain.Entities;
using Shoppu.Domain.Enums;
using Shoppu.Domain.ViewModels;
using System.Globalization;

namespace Shoppu.Application.Helpers
{
    public static class ProductQueryHelpers
    {
        public static List<int> GetBelongingCategoriesIds(List<ProductCategory> allCategories, string selectedCategoryUrl)
        {
            var belongingCategoriesIds = new List<int>();

            var selectedCategory = allCategories.FirstOrDefault(c => c.UrlName.Equals(selectedCategoryUrl));
            belongingCategoriesIds.Add(selectedCategory.Id);
            GetSubcategoryIds(selectedCategory.SubCategories);

            void GetSubcategoryIds(List<ProductCategory>? subCategories)
            {
                if (subCategories != null && subCategories.Count() > 0)
                {
                    foreach (var subcategory in subCategories)
                    {
                        belongingCategoriesIds.Add(subcategory.Id);
                        GetSubcategoryIds(subcategory.SubCategories);
                    }
                }
            }
            return belongingCategoriesIds;
        }

        public static IQueryable<ProductVariant> FilterSizes(this IQueryable<ProductVariant> query, List<int> sizes)
        {
            if (sizes != null && sizes.Count() > 0)
            {
                query = query.Where(pv => pv.Sizes.Where(pvs => sizes.Contains(pvs.SizeId)).Count() > 0);
            }
            return query;
        }

        public static IQueryable<ProductVariant> FilterVariants(this IQueryable<ProductVariant> query, List<int> variants)
        {
            if (variants != null && variants.Count() > 0)
            {
                query = query.Where(pv => variants.Contains(pv.VariantId));
            }
            return query;
        }

        public static IQueryable<ProductVariant> FilterPrices(this IQueryable<ProductVariant> query, string? minPrice, string? maxPrice)
        {
            bool minPriceSet = false;
            bool maxPriceSet = false;
            decimal minPriceParsed;
            decimal maxPriceParsed;
            minPriceSet = Decimal.TryParse(minPrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out minPriceParsed);
            maxPriceSet = Decimal.TryParse(maxPrice, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out maxPriceParsed);

            if (minPriceSet && maxPriceSet)
            {
                return query.Where(pv => ((pv.Price ?? pv.Product.Price) >= minPriceParsed) && ((pv.Price ?? pv.Product.Price) <= maxPriceParsed));
            }
            if (minPriceSet)
            {
                return query.Where(pv => (pv.Price ?? pv.Product.Price) >= minPriceParsed);
            }
            if (maxPriceSet)
            {
                return query.Where(pv => (pv.Price ?? pv.Product.Price) <= maxPriceParsed);
            }
            return query;
        }

        public static IQueryable<ProductVariant> SortProducts(this IQueryable<ProductVariant> query, ProductSortBy? sortBy)
        {
            switch (sortBy)
            {
                case ProductSortBy.LeastRecent:
                    return query.OrderBy(pv => pv.DateSetAsAccessible);

                case ProductSortBy.PriceLowest:
                    return query.OrderBy(pv => pv.Price ?? pv.Product.Price);

                case ProductSortBy.PriceHighest:
                    return query.OrderByDescending(pv => pv.Price ?? pv.Product.Price);

                case ProductSortBy.AZ:
                    return query.OrderBy(pv => pv.Name ?? pv.Product.Name);

                case ProductSortBy.ZA:
                    return query.OrderByDescending(pv => pv.Name ?? pv.Product.Name);

                case ProductSortBy.MostRecent:
                default:
                    return query.OrderByDescending(pv => pv.DateSetAsAccessible);
            }
        }

        public static List<Size> OrderSizesByNames(this List<Size> sizes)
        {
            return sizes
                .OrderByDescending(s => s.Name == "XS")
                .ThenByDescending(s => s.Name == "S")
                .ThenByDescending(s => s.Name == "M")
                .ThenByDescending(s => s.Name == "L")
                .ThenByDescending(s => s.Name == "XL")
                .ThenByDescending(s => s.Name == "XXL")
                .ThenByDescending(s => s.Name == "3XL")
                .ThenByDescending(s => s.Name == "4XL")
                .ThenBy(s => s.Name)
                .ToList();
        }

        public static IQueryable<BrowseDataProductItemViewModel> SelectBrowseData(this IQueryable<ProductVariant> query)
        {
            return query
            .Select(pv => new BrowseDataProductItemViewModel
            {
                ProductId = pv.Product.Id,
                ProductName = pv.Product.Name,
                CategoryId = pv.Product.ProductCategory.Id,
                CategoryName = pv.Product.ProductCategory.Name,
                Slug = pv.Slug,
                Price = pv.Product.Price,
                VariantId = pv.Id,
                VariantName = pv.Name,
                VariantPrice = pv.Price,
                ImagePaths = pv.Images
                        .Take(2)
                        .Select(pvi => pvi.ImageSource)
                        .ToList(),
                DateSetAsAccessible = pv.DateSetAsAccessible

            })
                .OrderBy(pv => pv.DateSetAsAccessible)
                .Take(8);
        }

    }
}