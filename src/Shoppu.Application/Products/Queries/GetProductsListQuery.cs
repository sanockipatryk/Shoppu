using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Products.Queries
{
    public record GetProductsListQuery() : IRequest<List<Product>>;

    public class GetProductListQueryHandler : IRequestHandler<GetProductsListQuery, List<Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
        {
            var productList = await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Variants)
                .ThenInclude(v => v.Variant)
                .Include(p => p.Variants)
                .ThenInclude(p => p.Images)
                .Include(p => p.Variants)
                .ThenInclude(p => p.Sizes)
                .ThenInclude(p => p.Size)
                .ToListAsync();
            return productList;
        }
    }
}
