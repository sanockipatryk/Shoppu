using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Sizes.Commads
{
    public record AddExistingSizesAndPossibleSizesForProductCommand(int VariantId, List<AddVariantSizesViewModel> AddVariantSizes) : IRequest<NotificationWithUrlData>;

    public class AddExistingSizesAndPossibleSizesForProductCommandHandler : IRequestHandler<AddExistingSizesAndPossibleSizesForProductCommand, NotificationWithUrlData>
    {
        private readonly IApplicationDbContext _context;

        public AddExistingSizesAndPossibleSizesForProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationWithUrlData> Handle(AddExistingSizesAndPossibleSizesForProductCommand request, CancellationToken cancellationToken)
        {
            if (request.AddVariantSizes.Count() > 0)
            {
                if (!request.AddVariantSizes.Any(vs => vs.AlreadyExists && vs.QuantityToAdd > 0)
                && !request.AddVariantSizes.Any(vs => !vs.AlreadyExists && vs.AddSizeAsVariantOption && vs.Quantity > 0))
                {
                    return new NotificationWithUrlData
                    {
                        Notification = new NotificationMessageViewModel
                        {
                            StatusType = Domain.Enums.StatusType.Info,
                            Message = "No changes were selected."
                        },
                    };
                }
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
                        }
                    })
                    .FirstOrDefaultAsync();

                if (variantFromDb != null)
                {

                    var existingSizesFromDb = await _context.ProductVariantSizes
                        .Where(pvs => pvs.ProductVariantId == request.VariantId)
                        .ToListAsync();

                    foreach (var sizeToAdd in request.AddVariantSizes.Where(s => s.AlreadyExists && s.QuantityToAdd != null && s.QuantityToAdd > 0))
                    {
                        existingSizesFromDb.FirstOrDefault(s => s.SizeId == sizeToAdd.SizeId).Quantity += (int)sizeToAdd.QuantityToAdd;
                    }

                    foreach (var newSize in request.AddVariantSizes.Where(s => !s.AlreadyExists && s.AddSizeAsVariantOption))
                    {
                        await _context.ProductVariantSizes.AddAsync(new ProductVariantSize
                        {
                            ProductVariantId = request.VariantId,
                            Quantity = (int)newSize.Quantity,
                            SizeId = newSize.SizeId
                        });
                    }

                    await _context.SaveChangesAsync(cancellationToken);

                    return new NotificationWithUrlData
                    {
                        Notification = new NotificationMessageViewModel
                        {
                            StatusType = Domain.Enums.StatusType.Success,
                            Message = "Successfully added sizes to product variant."
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
                    Message = "Variant has no specified initial sizes."
                }
            };
        }
    }
}