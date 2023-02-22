using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Colors.Commands
{
    public record CreateColorVariantCommand(CreateColorViewModel NewColor) : IRequest<NotificationMessageViewModel>;

    public class CreateColorVariantCommandHandler : IRequestHandler<CreateColorVariantCommand, NotificationMessageViewModel>
    {
        private readonly IApplicationDbContext _context;
        public CreateColorVariantCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationMessageViewModel> Handle(CreateColorVariantCommand request, CancellationToken cancellationToken)
        {
            bool colorExistsWithSameName = await _context.Variants.AnyAsync(pc => pc.Name.ToLower().Equals(request.NewColor.Name.ToLower()));

            if (colorExistsWithSameName)
            {
                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Danger,
                    Message = "Color variant with the same Name already exists."
                };
            }

            bool colorExistsWithSameHEX = await _context.Variants.AnyAsync(pc => pc.HEXColor.ToLower().Equals(request.NewColor.HEXColor.ToLower()));

            if (colorExistsWithSameHEX)
            {
                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Danger,
                    Message = "Color variant with the same HEX code already exists."
                };
            }

            var colorName = request.NewColor.Name.ToLower();
            var colorNameCapitalized = char.ToUpper(colorName[0]) + colorName.Substring(1);

            await _context.AddAsync(new Variant
            {
                Name = colorNameCapitalized,
                HEXColor = request.NewColor.HEXColor.ToUpper()
            });

            await _context.SaveChangesAsync(cancellationToken);

            return new NotificationMessageViewModel
            {
                StatusType = Domain.Enums.StatusType.Success,
                Message = $"Color '{colorNameCapitalized}' has been added."
            };
        }
    }
}