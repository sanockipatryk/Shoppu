using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Sizes.Queries
{
    public record GetAllSizeTypesQuery : IRequest<List<SizeType>>;

    public class GetAllSizeTypesQueryHandler : IRequestHandler<GetAllSizeTypesQuery, List<SizeType>>
    {
        private readonly IApplicationDbContext _context;
        public GetAllSizeTypesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SizeType>> Handle(GetAllSizeTypesQuery request, CancellationToken cancellationToken)
        {
            var allSizeTypes = await _context.SizeTypes
                .Select(st => new SizeType
                {
                    Id = st.Id,
                    Name = st.Name,
                })
                .ToListAsync();

            return allSizeTypes;
        }
    }
}