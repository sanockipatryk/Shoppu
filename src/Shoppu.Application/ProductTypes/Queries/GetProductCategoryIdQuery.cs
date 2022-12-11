using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.ProductTypes.Queries
{

    public record GetProductCategoryIdQuery(string CategoryUrl) : IRequest<int?>;

    public class GetProductCategoryIdQueryHandler : IRequestHandler<GetProductCategoryIdQuery, int?>
    {
        private readonly IApplicationDbContext _context;

        public GetProductCategoryIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int?> Handle(GetProductCategoryIdQuery request, CancellationToken cancellationToken)
        {
            ProductCategory? category = await _context.ProductCategories.FirstOrDefaultAsync(p => p.UrlName.Equals(request.CategoryUrl));
            if (category != null)
            {
                return category.Id;
            }
            return null;
        }
    }
}