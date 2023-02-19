using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Products.Commands.ManageProduct
{
    public record SetProductVariantAccessibleCommand(int Id) : IRequest<string>;

    public class SetProductVariantAccessibleCommandHandler : IRequestHandler<SetProductVariantAccessibleCommand, string>
    {
        private readonly IApplicationDbContext _context;

        public SetProductVariantAccessibleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(SetProductVariantAccessibleCommand request, CancellationToken cancellationToken)
        {
            var productVariant = await _context.ProductVariants
                .Where(pv => pv.Id == request.Id)
                .Include(pv => pv.Product)
                .Include(pv => pv.Sizes)
                .FirstOrDefaultAsync();

            if (productVariant != null && !productVariant.IsAccessible && productVariant?.Sizes?.Count() > 0)
            {
                productVariant.IsAccessible = true;
                productVariant.DateSetAsAccessible = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                return productVariant.Product.Code;
            }
            return null;
        }
    }
}