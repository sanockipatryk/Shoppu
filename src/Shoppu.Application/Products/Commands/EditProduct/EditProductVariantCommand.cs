using System.Globalization;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Commands.EditProduct
{
    public record EditProductVariantCommand(EditProductVariantViewModel ProductVariant) : IRequest<NotificationWithUrlAndProductVariantData>;

    public class EditProductVariantCommandHandler : IRequestHandler<EditProductVariantCommand, NotificationWithUrlAndProductVariantData>
    {
        private readonly IApplicationDbContext _context;
        public EditProductVariantCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationWithUrlAndProductVariantData> Handle(EditProductVariantCommand request, CancellationToken cancellationToken)
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

                var productVariantExistsWithSameCode = await _context.ProductVariants
                    .AnyAsync(pv => pv.Id != productVariantFromDb.Id && pv.Code.Equals(codeStringBuilder.ToString().ToUpper()));

                if (!productVariantExistsWithSameCode)
                {
                    if (
                        ((productVariantFromDb.Name != null && request.ProductVariant.Name != null &&
                        productVariantFromDb.Name.ToLower().Equals(request.ProductVariant.Name.ToLower()))
                        || productVariantFromDb.Name == null && request.ProductVariant.Name == null)
                        && productVariantFromDb.Price.Equals(request.ProductVariant.Price)
                        && productVariantFromDb.VariantId.Equals(request.ProductVariant.VariantId)
                        && productVariantFromDb.Code.Equals(codeStringBuilder.ToString().ToUpper())
                    )
                    {
                        return new NotificationWithUrlAndProductVariantData
                        {
                            Notification = new NotificationMessageViewModel
                            {
                                StatusType = Domain.Enums.StatusType.Info,
                                Message = "No changes were made."
                            },
                            CategoryUrl = productVariantFromDb.Product.ProductCategory.UrlName,
                            ProductCode = productVariantFromDb.Product.Code,
                            ProductVariantId = productVariantFromDb.Id,
                            ProductVariantCode = productVariantFromDb.Code,
                        };
                    }

                    productVariantFromDb.Name = request.ProductVariant.Name;
                    if (request.ProductVariant.Price != null)
                    {
                        productVariantFromDb.Price = Decimal.Parse(request.ProductVariant.Price, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        productVariantFromDb.Price = null;
                    }

                    productVariantFromDb.Code = codeStringBuilder.ToString().ToUpper();
                    productVariantFromDb.VariantId = request.ProductVariant.VariantId;

                    await _context.SaveChangesAsync(cancellationToken);

                    return new NotificationWithUrlAndProductVariantData
                    {
                        Notification = new NotificationMessageViewModel
                        {
                            StatusType = Domain.Enums.StatusType.Success,
                            Message = "Variant was updated."
                        },
                        CategoryUrl = productVariantFromDb.Product.ProductCategory.UrlName,
                        ProductCode = productVariantFromDb.Product.Code,
                        ProductVariantId = productVariantFromDb.Id,
                        ProductVariantCode = productVariantFromDb.Code,
                    };
                }
                else
                {
                    return new NotificationWithUrlAndProductVariantData
                    {
                        Notification = new NotificationMessageViewModel
                        {
                            StatusType = Domain.Enums.StatusType.Danger,
                            Message = "Variant of this product with the same code already exists."
                        }
                    };
                }
            }

            return new NotificationWithUrlAndProductVariantData
            {
                Notification = new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Warning,
                    Message = "Something went wrong. Try again."
                }
            };
        }
    }
}