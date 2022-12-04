using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppu.Application.ProductTypes.Queries
{
    public record GetVariantListQuery() : IRequest<List<Variant>>;

    public class GetVariantListQueryHandler : IRequestHandler<GetVariantListQuery, List<Variant>>
    {
        private readonly IApplicationDbContext _context;
        public GetVariantListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Variant>> Handle(GetVariantListQuery request, CancellationToken cancellationToken)
        {
            return await _context.Variants.ToListAsync(cancellationToken);
        }
    }
}
