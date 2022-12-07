using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.Extensions;
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
            var productFromDb = await _context.Products
                .Where(p => p.Id == request.ProductVariant.ProductId)
                .Select(p => new Product
                {
                    Id = p.Id,
                    BaseSlug = p.BaseSlug
                })
                .FirstOrDefaultAsync();

            if (productFromDb != null)
            {
                var newVariantSlug = productFromDb.BaseSlug;

                do
                {
                    string newSlug = newVariantSlug;
                    newSlug = newSlug.AppendRandomDigits(3);
                    if (!await _context.ProductVariants.AnyAsync(pv => pv.Slug.Equals(newSlug)))
                    {
                        newVariantSlug = newSlug;
                        break;
                    }
                } while (true);

                var productVariant = new ProductVariant
                {
                    ProductId = request.ProductVariant.ProductId,
                    VariantId = request.ProductVariant.VariantId,
                    Name = request.ProductVariant.Name,
                    Price = request.ProductVariant.Price != null ? Decimal.Parse(request.ProductVariant.Price, CultureInfo.InvariantCulture) : null,
                    Slug = newVariantSlug,
                };
                var productVariantFromDb = await _context.ProductVariants.AddAsync(productVariant);
                await _context.SaveChangesAsync(cancellationToken);

                var createdProductVariant = await _context.ProductVariants
                    .Include(p => p.Product)
                    .ThenInclude(pr => pr.ProductCategory)
                    .FirstOrDefaultAsync(p => p.Id == productVariantFromDb.Entity.Id);

                return createdProductVariant;
            }
            return null;
        }
    }
}
