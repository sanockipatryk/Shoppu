﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Application.Helpers;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Queries
{
    public record GetProductsListQuery(string? CategoryUrl, ManageProductsFiltersViewModel Filters, PaginationViewModel Pagination) : IRequest<ManageDataViewModel>;

    public class GetProductListQueryHandler : IRequestHandler<GetProductsListQuery, ManageDataViewModel>
    {
        private readonly IApplicationDbContext _context;

        public GetProductListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ManageDataViewModel> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            PaginationViewModel newPagination = new PaginationViewModel
            {
                Page = request.Pagination.Page,
                ItemsPerPage = request.Pagination.ItemsPerPage
            };

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


            var productListFromDb = await _context.Products
               .Where(p => belongingCategoriesIds.Contains(p.ProductCategory.Id))
               .Where(p => request.Filters.Code == null || p.Code.ToLower().Contains(request.Filters.Code.ToLower()))
               .Where(p => request.Filters.Name == null || p.Name.ToLower().Contains(request.Filters.Name.ToLower()))
               .Where(p => request.Filters.Gender == null || p.Gender == request.Filters.Gender)
               .Where(p => request.Filters.WithoutAnyVariants == false || p.Variants.Count() == 0)
               .Where(p => accessibility == null || (p.Variants.Where(pv => pv.IsAccessible == accessibility).Count() > 0))
               .Where(p => request.Filters.WithoutSpecifiedSizes == false || (p.Variants.Where(pv => pv.Sizes.Count() == 0).Count() > 0))
               .OrderBy(p => p.Name)
               .ApplyPagination(ref newPagination)
               .Select(p => new Product
               {
                   Id = p.Id,
                   Name = p.Name,
                   Gender = p.Gender,
                   Price = p.Price,
                   Code = p.Code,
                   ProductCategory = new ProductCategory
                   {
                       Id = p.ProductCategory.Id,
                       Name = p.ProductCategory.Name,
                       UrlName = p.ProductCategory.UrlName,
                       SpecificGender = p.ProductCategory.SpecificGender
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
                            Code = pv.Code,
                            Images = pv.Images.Select(pvi => new ProductVariantImage
                            {
                                ImageSource = pvi.ImageSource
                            }).ToList(),
                            Sizes = pv.Sizes.Select(pvs => new ProductVariantSize
                            {
                                Id = pv.Id,
                                Quantity = pvs.Quantity,
                                Size = new Size
                                {
                                    Name = pvs.Size.Name
                                },
                                OrderItems = pvs.OrderItems.Select(oi => new OrderItem
                                {
                                    Id = oi.Id,
                                    ProductVariantSizeId = pvs.Id,
                                    Quantity = oi.Quantity,
                                }).ToList(),
                                QuantitySold = pvs.OrderItems != null ? pvs.OrderItems.Sum(oi => oi.Quantity) : 0
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

            return new ManageDataViewModel
            {
                Products = productListFromDb,
                Pagination = newPagination,
                AppliedFilters = request.Filters
            };
        }
    }
}
