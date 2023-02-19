using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Sizes.Commads
{
    public record CreateProductVariantSizesCommand(int VariantId, List<CreateProductVariantSizeViewModel> ProductVariantSizes) : IRequest<bool>;

    public class CreateProductVariantSizesCommandHandler : IRequestHandler<CreateProductVariantSizesCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        public CreateProductVariantSizesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateProductVariantSizesCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductVariantSizes.Where(p => p.AddSizeAsVariantOption).Count() > 0)
            {
                var variantFromDb = await _context.ProductVariants.FirstOrDefaultAsync(p => p.Id == request.VariantId);
                if (variantFromDb != null)
                {
                    foreach (var productVariantSize in request.ProductVariantSizes.Where(p => p.AddSizeAsVariantOption))
                    {
                        await _context.ProductVariantSizes.AddAsync(new ProductVariantSize
                        {
                            ProductVariantId = variantFromDb.Id,
                            SizeId = productVariantSize.SizeId,
                            Quantity = (int)productVariantSize.Quantity
                        });
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                    return true;
                }
            }
            return false;
        }
    }
}