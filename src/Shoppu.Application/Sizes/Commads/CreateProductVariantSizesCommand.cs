using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Sizes.Commads
{
    public record CreateProductVariantSizesCommand(int VariantId, List<CreateProductVariantSizeViewModel> ProductVariantSizes) : IRequest<NotificationWithUrlData>;

    public class CreateProductVariantSizesCommandHandler : IRequestHandler<CreateProductVariantSizesCommand, NotificationWithUrlData>
    {
        private readonly IApplicationDbContext _context;
        public CreateProductVariantSizesCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationWithUrlData> Handle(CreateProductVariantSizesCommand request, CancellationToken cancellationToken)
        {
            if (request.ProductVariantSizes.Where(pvs => pvs.AddSizeAsVariantOption).Count() > 0)
            {
                var variantFromDb = await _context.ProductVariants
                .Where(pv => pv.Id == request.VariantId)
                .Select(pv => new ProductVariant
                {
                    Id = pv.Id,
                    Product = new Product
                    {
                        Id = pv.Product.Id,
                        Code = pv.Product.Code,
                        ProductCategory = new ProductCategory
                        {
                            UrlName = pv.Product.ProductCategory.UrlName
                        }
                    },
                })
                .FirstOrDefaultAsync();

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

                    return new NotificationWithUrlData
                    {
                        Notification = new NotificationMessageViewModel
                        {
                            StatusType = Domain.Enums.StatusType.Success,
                        },
                        CategoryUrl = variantFromDb.Product.ProductCategory.UrlName,
                        ProductCode = variantFromDb.Product.Code
                    };
                }
                else
                {
                    return new NotificationWithUrlData
                    {
                        Notification = new NotificationMessageViewModel
                        {
                            StatusType = Domain.Enums.StatusType.Danger,
                            Message = "Something went wrong."
                        }
                    };
                }
            }
            return new NotificationWithUrlData
            {
                Notification = new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Danger,
                    Message = "No sizes were selected to add."
                }
            };
        }
    }
}