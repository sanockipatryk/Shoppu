using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Sizes.Commads
{
    public record AddExistingSizesAndPossibleSizesForProductCommand(int VariantId, List<AddVariantSizesViewModel> AddVariantSizes) : IRequest<NotificationMessageViewModel>;

    public class AddExistingSizesAndPossibleSizesForProductCommandHandler : IRequestHandler<AddExistingSizesAndPossibleSizesForProductCommand, NotificationMessageViewModel>
    {
        private readonly IApplicationDbContext _context;

        public AddExistingSizesAndPossibleSizesForProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationMessageViewModel> Handle(AddExistingSizesAndPossibleSizesForProductCommand request, CancellationToken cancellationToken)
        {
            if (request.AddVariantSizes.Count() > 0)
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

                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Success,
                    Message = "Successfully added sizes to product variant."
                };
            }

            return new NotificationMessageViewModel
            {
                StatusType = Domain.Enums.StatusType.Danger,
                Message = "Variant has no specified initial sizes."
            };
        }
    }
}