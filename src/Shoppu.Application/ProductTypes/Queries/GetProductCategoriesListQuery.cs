using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.ProductTypes.Queries
{
    public record GetProductCategoriesListQuery(bool CheckForExistingProducts = false) : IRequest<List<ProductCategory>>;

    public class GetProductCategoriesListQueryHandler : IRequestHandler<GetProductCategoriesListQuery, List<ProductCategory>>
    {
        private readonly IApplicationDbContext _context;
        public GetProductCategoriesListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductCategory>> Handle(GetProductCategoriesListQuery request, CancellationToken cancellationToken)
        {
            var productCategories = await _context.ProductCategories.Include(pc => pc.ParentCategory).ToListAsync();

            if (request.CheckForExistingProducts)
            {
                var productCategoriesWithoutProducts = await _context.ProductCategories
                    .Where(pc => !pc.Products.Any())
                    .ToListAsync();

                foreach (var category in productCategoriesWithoutProducts)
                {
                    productCategories.FirstOrDefault(pc => pc.Id == category.Id).HasNoExistingProducts = true;
                }
            }

            return productCategories;
        }
    }
}
