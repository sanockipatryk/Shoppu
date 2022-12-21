using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Application.Helpers;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Queries
{
    public record GetProductsListQuery(string? CategoryUrl, ManageProductsFiltersViewModel Filters) : IRequest<List<Product>>;

    public class GetProductListQueryHandler : IRequestHandler<GetProductsListQuery, List<Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            bool? accessibility = null;
            if (request.Filters.AccessibilityStatus != null)
            {
                switch (request.Filters.AccessibilityStatus)
                {
                    case Domain.Enums.ProductAccessibilityStatus.Accessible:
                        accessibility = true;
                        break;
                    case Domain.Enums.ProductAccessibilityStatus.Unaccessible:
                        accessibility = false;
                        break;
                }
            }
            var allCategories = await _context.ProductCategories.Include(p => p.SubCategories).ToListAsync();

            var belongingCategoriesIds = ProductQueryHelpers.GetBelongingCategoriesIds(allCategories, request.CategoryUrl);


            var productList2 = await _context.Products
               .Where(p => belongingCategoriesIds.Contains(p.ProductCategory.Id))
               .Where(p => request.Filters.Name == null || p.Name.ToLower().Contains(request.Filters.Name.ToLower()))
               .Where(p => request.Filters.Gender == null || p.Gender == request.Filters.Gender)
               .Where(p => request.Filters.WithoutAnyVariants == false || p.Variants.Count() == 0)
               .Where(p => accessibility == null || (p.Variants.Where(pv => pv.IsAccessible == accessibility).Count() > 0))
               .Where(p => request.Filters.WithoutSpecifiedSizes == false || (p.Variants.Where(pv => pv.Sizes.Count() == 0).Count() > 0))
               .Select(p => new Product
               {
                   Id = p.Id,
                   Name = p.Name,
                   Gender = p.Gender,
                   Price = p.Price,
                   ProductCategory = new ProductCategory
                   {
                       Id = p.ProductCategory.Id,
                       Name = p.ProductCategory.Name,
                       UrlName = p.ProductCategory.UrlName,
                   },
                   Variants = p.Variants
                        .Where(pv => request.Filters.WithoutSpecifiedSizes == false || pv != null && pv.Sizes.Count() == 0)
                        .Where(pv => request.Filters.WithoutSpecifiedSizes == false || pv != null && pv.Sizes.Count() == 0)
                        .Where(pv => accessibility == null || pv.IsAccessible == accessibility)
                        .Select(pv => new ProductVariant
                        {
                            Id = pv.Id,
                            Name = pv.Name,
                            Price = pv.Price,
                            Slug = pv.Slug,
                            Images = pv.Images.Select(pvi => new ProductVariantImage
                            {
                                ImageSource = pvi.ImageSource
                            }).ToList(),
                            Sizes = pv.Sizes.Select(pvs => new ProductVariantSize
                            {
                                Quantity = pvs.Quantity,
                                Size = new Size
                                {
                                    Name = pvs.Size.Name
                                },
                            }).ToList(),
                            Variant = new Variant
                            {
                                Id = pv.Variant.Id,
                                Name = pv.Variant.Name
                            },
                            IsAccessible = pv.IsAccessible
                        }).ToList()
               })
               .ToListAsync();


            return productList2;
        }
    }
}
