using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Application.Queries.Products;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Handlers.Products
{
    public class GetProductListHandler : IRequestHandler<GetProductListQuery, List<Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductListHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var productList = await _context.Products.ToListAsync();
            return productList;
        }
    }
}
