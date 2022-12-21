using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Application.Helpers;
using Shoppu.Domain.Entities;
using Shoppu.Domain.Enums;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Queries.Shop
{
    public record GetBrowseProductsListQuery(string CategoryUrl, string Gender, BrowseProductsFiltersViewModel Filters) : IRequest<BrowseDataViewModel>;
    public class GetBrowseProductsListQueryHandler : IRequestHandler<GetBrowseProductsListQuery, BrowseDataViewModel>
    {
        private readonly IApplicationDbContext _context;

        public GetBrowseProductsListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BrowseDataViewModel> Handle(GetBrowseProductsListQuery request, CancellationToken cancellationToken)
        {

            var allCategories = await _context.ProductCategories.Include(p => p.SubCategories).ToListAsync();

            var belongingCategoriesIds = ProductQueryHelpers.GetBelongingCategoriesIds(allCategories, request.CategoryUrl);

            var minPrice = _context.ProductVariants
                .Where(pv => pv.IsAccessible && belongingCategoriesIds.Contains(pv.Product.ProductCategoryId))
                .Min(pv => (pv.Price != null ? pv.Price : pv.Product.Price));
            var maxPrice = _context.ProductVariants
                .Where(pv => pv.IsAccessible && belongingCategoriesIds.Contains(pv.Product.ProductCategoryId))
                .Max(pv => (pv.Price != null ? pv.Price : pv.Product.Price));

            var variants = await _context.ProductVariants
                .Where(pv => pv.IsAccessible)
                .Where(pv => request.Gender.ToLower().Equals("woman")
                    ? (pv.Product.Gender == ProductGender.Woman || pv.Product.Gender == ProductGender.Unisex)
                    : (pv.Product.Gender == ProductGender.Man || pv.Product.Gender == ProductGender.Unisex))
                .FilterSizes(request.Filters.Sizes)
                .FilterVariants(request.Filters.Variants)
                .FilterPrices(request.Filters.MinPrice, request.Filters.MaxPrice)
                .Where(pv => belongingCategoriesIds.Contains(pv.Product.ProductCategory.Id))
                .Select(pv => new ProductVariant
                {
                    Id = pv.Id,
                    Name = pv.Name,
                    Price = pv.Price,
                    DateSetAsAccessible = pv.DateSetAsAccessible,
                    Product = new Product
                    {
                        Id = pv.Product.Id,
                        Name = pv.Product.Name,
                        ProductCategory = new ProductCategory
                        {
                            Id = pv.Product.ProductCategory.Id,
                            Name = pv.Product.ProductCategory.Name,
                        },
                        Price = pv.Product.Price,
                    },
                    Slug = pv.Slug,
                    Images = pv.Images,
                })
                .SortProducts(request.Filters.SortBy)
                .ToListAsync();

            var possibleColors = await _context.ProductVariants
                .Where(pv => pv.IsAccessible && belongingCategoriesIds.Contains(pv.Product.ProductCategoryId))
                .Select(pv => new Variant
                {
                    Id = pv.Variant.Id,
                    Name = pv.Variant.Name,
                    HEXColor = pv.Variant.HEXColor
                })
                .GroupBy(v => new { v.Id, v.Name }).Select(g => g.First())
                .ToListAsync();

            var possibleColorsOrdered = possibleColors.OrderBy(c => c.Name);

            var possibleSizes = await _context.ProductVariantSizes
                .Where(pvs => pvs.ProductVariant.IsAccessible && belongingCategoriesIds.Contains(pvs.ProductVariant.Product.ProductCategoryId))
                .Select(pvs => new Size
                {
                    Id = pvs.Size.Id,
                    Name = pvs.Size.Name
                })
                .GroupBy(s => new { s.Id, s.Name }).Select(g => g.First())
                .ToListAsync();

            var possibleSizesOrdered = possibleSizes.OrderSizesByNames();


            var browseData = new BrowseDataViewModel();
            browseData.Products = new List<BrowseDataProductItemViewModel>();

            foreach (var variant in variants)
            {
                List<string> ImagePaths = new List<string>();
                foreach (var image in variant.Images)
                {
                    ImagePaths.Add(image.ImageSource);
                }

                browseData.Products.Add(new BrowseDataProductItemViewModel
                {
                    ProductId = variant.Product.Id,
                    ProductName = variant.Product.Name,
                    VariantId = variant.Id,
                    VariantName = variant.Name,
                    VariantPrice = variant.Price,
                    CategoryId = variant.Product.ProductCategory.Id,
                    CategoryName = variant.Product.ProductCategory.Name,
                    Slug = variant.Slug,
                    Price = variant.Product.Price,
                    ImagePaths = ImagePaths
                });
            }

            browseData.MinPrice = minPrice ?? 0;
            browseData.MaxPrice = maxPrice ?? 0;
            browseData.PossibleSizes = possibleSizes;
            browseData.PossibleVariants = possibleColors;

            return browseData;

        }
    }
}