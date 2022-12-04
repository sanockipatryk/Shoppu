using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Queries.Shop
{
    public record GetBrowseProductsListQuery(int CategoryId) : IRequest<BrowseProductViewModel>;
    public class GetBrowseProductsListQueryHandler : IRequestHandler<GetBrowseProductsListQuery, BrowseProductViewModel>
    {
        private readonly IApplicationDbContext _context;

        public GetBrowseProductsListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BrowseProductViewModel> Handle(GetBrowseProductsListQuery request, CancellationToken cancellationToken)
        {

            var allCategories = await _context.ProductCategories.Include(p => p.SubCategories).ToListAsync();

            var belongingCategoriesIds = new List<int>();

            var selectedCategory = allCategories.FirstOrDefault(c => c.Id == request.CategoryId);
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

            var variants = await _context.ProductVariants
                .Where(pv => belongingCategoriesIds.Contains(pv.Product.ProductCategory.Id))
                .Select(pv => new ProductVariant
            {
                Id = pv.Id,
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
                Images = pv.Images,
            }).ToListAsync();

            var browseProducts = new BrowseProductViewModel();
            browseProducts.Products = new List<BrowseProductItemViewModel>();

            foreach (var variant in variants)
            {
                List<string> ImagePaths = new List<string>();
                foreach (var image in variant.Images)
                {
                    ImagePaths.Add(image.ImageSource);
                }

                browseProducts.Products.Add(new BrowseProductItemViewModel
                {
                    ProductId = variant.Product.Id,
                    ProductName = variant.Product.Name,
                    VariantId = variant.Id,
                    VariantName = variant.Id.ToString(),
                    CategoryId = variant.Product.ProductCategory.Id,
                    CategoryName = variant.Product.ProductCategory.Name,
                    Price = variant.Product.Price,
                    ImagePaths = ImagePaths
                });
            }

            return browseProducts;

        }
    }
}