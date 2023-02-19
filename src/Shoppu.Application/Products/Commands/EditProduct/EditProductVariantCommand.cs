using System.Globalization;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Commands.EditProduct
{
    public record EditProductVariantCommand(EditProductVariantViewModel ProductVariant) : IRequest<ProductVariant>;

    public class EditProductVariantCommandHandler : IRequestHandler<EditProductVariantCommand, ProductVariant>
    {
        private readonly IApplicationDbContext _context;
        public EditProductVariantCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductVariant> Handle(EditProductVariantCommand request, CancellationToken cancellationToken)
        {
            var productVariantFromDb = await _context.ProductVariants
                .Where(pv => pv.Id == request.ProductVariant.ProductVariantId && !pv.IsAccessible)
                .Include(pv => pv.Product)
                .ThenInclude(p => p.ProductCategory)
                .FirstOrDefaultAsync();

            if (productVariantFromDb != null)
            {
                StringBuilder codeStringBuilder = new StringBuilder(productVariantFromDb.Product.Code);
                codeStringBuilder.Append("/").Append(request.ProductVariant.CodeAddition);

                productVariantFromDb.Name = request.ProductVariant.Name;
                if (request.ProductVariant.Price != null)
                {
                    productVariantFromDb.Price = Decimal.Parse(request.ProductVariant.Price, CultureInfo.InvariantCulture);
                }
                productVariantFromDb.Code = codeStringBuilder.ToString();
                productVariantFromDb.VariantId = request.ProductVariant.VariantId;

                await _context.SaveChangesAsync(cancellationToken);

                return productVariantFromDb;
            }
            return null;
        }
    }
}