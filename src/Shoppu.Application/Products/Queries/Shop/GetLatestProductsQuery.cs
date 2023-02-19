using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.ViewModels;
using Shoppu.Domain.Enums;
using Shoppu.Application.Helpers;

namespace Shoppu.Application.Products.Queries.Shop
{
    public record GetLatestProductsQuery : IRequest<BrowseLatestProductsViewModel>;

    public class GetLatestProductsQueryHandler : IRequestHandler<GetLatestProductsQuery, BrowseLatestProductsViewModel>
    {
        private readonly IApplicationDbContext _context;

        public GetLatestProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<BrowseLatestProductsViewModel> Handle(GetLatestProductsQuery request, CancellationToken cancellationToken)
        {
            var latestWomanProducts = await _context.ProductVariants
                .Where(pv => pv.IsAccessible
                        && (pv.Product.Gender == ProductGender.Woman
                        || pv.Product.Gender == ProductGender.Unisex)
                )
                .SelectBrowseData()
                .ToListAsync();

            var latestManProducts = await _context.ProductVariants
                .Where(pv => pv.IsAccessible
                        && (pv.Product.Gender == ProductGender.Man
                        || pv.Product.Gender == ProductGender.Unisex)
                )
                .SelectBrowseData()
                .ToListAsync();

            return new BrowseLatestProductsViewModel
            {
                LatestWomanProducts = latestWomanProducts,
                LatestManProducts = latestManProducts,
            };
        }
    }
}