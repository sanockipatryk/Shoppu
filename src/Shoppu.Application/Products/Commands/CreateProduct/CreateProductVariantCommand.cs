using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Commands.CreateProduct
{
    public record CreateProductVariantCommand(CreateProductVariantViewModel ProductVariant) : IRequest<ProductVariant>;

    public class CreateProductVariantCommandHandler : IRequestHandler<CreateProductVariantCommand, ProductVariant>
    {
        private readonly IApplicationDbContext _context;
        public CreateProductVariantCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductVariant> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
        {
            var productVariant = new ProductVariant
            {
                ProductId = request.ProductVariant.ProductId,
                VariantId = request.ProductVariant.VariantId,
            };
            var productVariantFromDb = await _context.ProductVariants.AddAsync(productVariant);
            await _context.SaveChangesAsync(cancellationToken);

            var createdProductVariant = await _context.ProductVariants
                .Include(p => p.Product)
                .ThenInclude(pr => pr.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == productVariantFromDb.Entity.Id);

            return createdProductVariant;
        }
    }
}
