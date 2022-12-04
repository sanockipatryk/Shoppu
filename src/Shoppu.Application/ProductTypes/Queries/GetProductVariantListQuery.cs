using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.ProductTypes.Queries
{
    public record GetProductVariantListQuery(int id) : IRequest<List<ProductVariant>>;

    public class GetProductVariantListQueryHandler : IRequestHandler<GetProductVariantListQuery, List<ProductVariant>>
    {
        private readonly IApplicationDbContext _context;
        public GetProductVariantListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductVariant>> Handle(GetProductVariantListQuery request, CancellationToken cancellationToken)
        {
            var productVariants = await _context.ProductVariants.Where(p => p.ProductId == request.id).ToListAsync();
            return productVariants;
        }
    }
}
