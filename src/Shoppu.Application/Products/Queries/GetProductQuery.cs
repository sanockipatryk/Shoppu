using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Products.Queries
{
    public record GetProductQuery(int id) : IRequest<Product>;

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
    {
        private readonly IApplicationDbContext _context;
        public GetProductQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(p => p.Id == request.id);
            if(product != null)
            {
                return product;
            }
            return new Product();
        }
    }
}
