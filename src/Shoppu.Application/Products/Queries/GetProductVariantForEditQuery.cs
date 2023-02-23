using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Queries
{
    public record GetProductVariantForEditQuery(int ProductId, int VariantId) : IRequest<EditProductVariantViewModel>;

    public class GetProductVariantForEditQueryHandler : IRequestHandler<GetProductVariantForEditQuery, EditProductVariantViewModel>
    {
        private readonly IApplicationDbContext _context;

        public GetProductVariantForEditQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EditProductVariantViewModel> Handle(GetProductVariantForEditQuery request, CancellationToken cancellationToken)
        {
            var productVariantFromDb = await _context.ProductVariants
                .Where(pv => pv.Id == request.VariantId && pv.ProductId == request.ProductId)
                .Select(pv => new ProductVariant
                {
                    Id = pv.Id,
                    Product = new Product
                    {
                        Id = pv.Product.Id,
                        Code = pv.Product.Code,
                        ProductCategory = new ProductCategory
                        {
                            UrlName = pv.Product.ProductCategory.UrlName
                        }
                    },
                    Name = pv.Name,
                    Price = pv.Price,
                    Code = pv.Code,
                    IsAccessible = pv.IsAccessible,
                    Images = pv.Images.Select(pvi => new ProductVariantImage
                    {
                        ImageSource = pvi.ImageSource
                    }).ToList(),
                    VariantId = pv.VariantId
                })
                .FirstOrDefaultAsync();


            return new EditProductVariantViewModel
            {
                ProductVariantId = productVariantFromDb.Id,
                ProductId = productVariantFromDb.Product.Id,
                VariantId = productVariantFromDb.VariantId,
                Name = productVariantFromDb.Name,
                Price = productVariantFromDb.Price.ToString() ?? null,
                IsAccessible = productVariantFromDb.IsAccessible,
                ProductCode = productVariantFromDb.Product.Code,
                CodeAddition = productVariantFromDb.Code.Substring(productVariantFromDb.Code.IndexOf("/") + 1).ToString(),
                ImagePaths = productVariantFromDb.Images.Select(pvi => pvi.ImageSource).ToList(),
                ProductCategoryUrl = productVariantFromDb.Product.ProductCategory.UrlName
            };
        }
    }
}