using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Sizes.Queries
{
    public record GetPossibleSizesForProductQuery(int ProductId) : IRequest<List<Size>>;

    public class GetPossibleSizesForProductQueryHandler : IRequestHandler<GetPossibleSizesForProductQuery, List<Size>>
    {
        private readonly IApplicationDbContext _context;
        public GetPossibleSizesForProductQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Size>> Handle(GetPossibleSizesForProductQuery request, CancellationToken cancellationToken)
        {
            var productFromDb = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);
            if(productFromDb != null)
            {
                var possibleSizes = await _context.Sizes.Where(c => c.ProductCategoryId == productFromDb.ProductCategoryId).ToListAsync();
                return possibleSizes;
            }
            return new List<Size>();
        }
    }
}
