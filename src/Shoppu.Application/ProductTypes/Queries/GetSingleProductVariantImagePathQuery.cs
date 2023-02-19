using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;

namespace Shoppu.Application.ProductTypes.Queries
{
    public record GetSingleProductVariantImagePathQuery(int ProductVariantId) : IRequest<string>;

    public class GetSingleProductVariantImagePathQueryHandler : IRequestHandler<GetSingleProductVariantImagePathQuery, string>
    {
        private readonly IApplicationDbContext _context;
        public GetSingleProductVariantImagePathQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetSingleProductVariantImagePathQuery request, CancellationToken cancellationToken)
        {
            var productVariantImagePath = await _context.ProductVariantImages
                .Where(pvi => pvi.ProductVariantId == request.ProductVariantId)
                .Select(pvi => pvi.ImageSource)
                .FirstOrDefaultAsync();

            return productVariantImagePath;
        }
    }
}