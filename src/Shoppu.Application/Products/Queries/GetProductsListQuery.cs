using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
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
            // var productList = await _context.Products
            //     .Include(p => p.ProductCategory)
            //     .Include(p => p.Variants)
            //     .ThenInclude(v => v.Variant)
            //     .Include(p => p.Variants)
            //     .ThenInclude(p => p.Images)
            //     .Include(p => p.Variants)
            //     .ThenInclude(p => p.Sizes)
            //     .ThenInclude(p => p.Size)
            //     .ToListAsync();


            var productList2 = await _context.Products
            //    .Where(p => request.Filters.Name != null && p.Name.ToLower().Contains(request.Filters.Name))
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
                   Variants = p.Variants.Select(pv => new ProductVariant
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
