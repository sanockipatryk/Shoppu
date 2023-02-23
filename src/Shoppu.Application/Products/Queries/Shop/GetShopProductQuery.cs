using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.Enums;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Queries.Shop
{
    public record GetShopProductQuery(string Slug) : IRequest<ShopProductViewModel>;

    public class GetShopProductQueryHandler : IRequestHandler<GetShopProductQuery, ShopProductViewModel>
    {
        private readonly IApplicationDbContext _context;

        public GetShopProductQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShopProductViewModel> Handle(GetShopProductQuery request, CancellationToken cancellationToken)
        {
            var productData = await _context.ProductVariants
            .Where(pv => pv.Slug.Equals(request.Slug))
            .Select(pv => new ProductVariant
            {
                Id = pv.Id,
                Name = pv.Name,
                Price = pv.Price,
                Product = new Product
                {
                    Id = pv.Product.Id,
                    Name = pv.Product.Name,
                    Description = pv.Product.Description,
                    ProductCategory = new ProductCategory
                    {
                        Id = pv.Product.ProductCategory.Id,
                        Name = pv.Product.ProductCategory.Name,
                    },
                    Price = pv.Product.Price,
                },
                Images = pv.Images.Select(i => new ProductVariantImage
                {
                    ImageSource = i.ImageSource
                }).ToList(),
                Sizes = pv.Sizes.Select(s => new ProductVariantSize
                {
                    Id = s.Id,
                    Quantity = s.Quantity,
                    Size = new Size
                    {
                        Name = s.Size.Name,
                    }
                }).ToList(),
            })
            .FirstOrDefaultAsync();

            var shopProduct = new ShopProductViewModel();
            shopProduct.ImagePaths = new List<string>();

            foreach (var image in productData.Images)
            {
                shopProduct.ImagePaths.Add(image.ImageSource);
            }

            var productVariants = await _context.ProductVariants
            .Where(pv => pv.ProductId == productData.Product.Id && pv.IsAccessible)
            .Select(pv => new ColorVariantViewModel
            {
                VariantSlug = pv.Slug,
                ColorName = pv.Variant.Name,
                HEXColor = pv.Variant.HEXColor,
                IsSelected = pv.Id == productData.Id
            })
            .OrderBy(pv => pv.ColorName)
            .ToListAsync();

            shopProduct.ProductId = productData.Product.Id;
            shopProduct.ProductName = productData.Product.Name;
            shopProduct.ProductDescription = productData.Product.Description;
            shopProduct.VariantId = productData.Id;
            shopProduct.VariantName = productData.Name;
            shopProduct.Price = productData.Product.Price;
            shopProduct.VariantPrice = productData.Price;
            shopProduct.Sizes = productData.Sizes;
            shopProduct.ProductVariants = productVariants;


            return shopProduct;

        }
    }
}