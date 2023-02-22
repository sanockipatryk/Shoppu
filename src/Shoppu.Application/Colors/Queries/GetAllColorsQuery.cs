using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Colors.Queries
{
    public record GetAllColorsQuery : IRequest<List<Variant>>;

    public class GetAllColorsQueryHandler : IRequestHandler<GetAllColorsQuery, List<Variant>>
    {
        private readonly IApplicationDbContext _context;
        public GetAllColorsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Variant>> Handle(GetAllColorsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Variants.OrderBy(v => v.Name).ToListAsync();
        }
    }
}