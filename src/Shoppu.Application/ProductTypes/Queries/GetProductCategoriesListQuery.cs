using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.ProductTypes.Queries
{
    public record GetProductCategoriesListQuery() : IRequest<List<ProductCategory>>;

    public class GetProductCategoriesListQueryHandler : IRequestHandler<GetProductCategoriesListQuery, List<ProductCategory>>
    {
        private readonly IApplicationDbContext _context;
        public GetProductCategoriesListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductCategory>> Handle(GetProductCategoriesListQuery request, CancellationToken cancellationToken)
        {
            return await _context.ProductCategories.Include(pc => pc.ParentCategory).ToListAsync();
        }
    }
}
