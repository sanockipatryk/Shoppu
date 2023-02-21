using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.ProductTypes.Commands
{
    public record UpdateProductCategoryCommand(EditProductCategoryViewModel EditedCategory) : IRequest<NotificationMessageViewModel>;

    public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, NotificationMessageViewModel>
    {
        private readonly IApplicationDbContext _context;
        public UpdateProductCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationMessageViewModel> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryFromDb = await _context.ProductCategories
                .Where(pc => pc.Id == request.EditedCategory.CategoryId)
                .FirstOrDefaultAsync();

            if (categoryFromDb != null)
            {
                if (!request.EditedCategory.Name.Equals(categoryFromDb.Name))
                {
                    bool categoryExistsWithSameName = await _context.ProductCategories.AnyAsync(pc => pc.Name.ToLower().Equals(request.EditedCategory.Name.ToLower()));

                    if (categoryExistsWithSameName)
                    {
                        return new NotificationMessageViewModel
                        {
                            StatusType = Domain.Enums.StatusType.Danger,
                            Message = "Different Category with the same Name already exists."
                        };
                    }
                }

                if (!request.EditedCategory.UrlName.ToLower().Equals(categoryFromDb.UrlName))
                {
                    bool categoryExistsWithSameUrlName = await _context.ProductCategories.AnyAsync(pc => pc.UrlName.Equals(request.EditedCategory.UrlName.ToLower()));

                    if (categoryExistsWithSameUrlName)
                    {
                        return new NotificationMessageViewModel
                        {
                            StatusType = Domain.Enums.StatusType.Danger,
                            Message = "Different Category with the same Url Name already exists."
                        };
                    }
                }

                categoryFromDb.Name = request.EditedCategory.Name;
                categoryFromDb.UrlName = request.EditedCategory.UrlName.ToLower();

                _context.Update(categoryFromDb);

                await _context.SaveChangesAsync(cancellationToken);

                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Info,
                    Message = "Selected category was updated."
                };
            }

            return new NotificationMessageViewModel
            {
                StatusType = Domain.Enums.StatusType.Danger,
                Message = "Selected category could not be updated."
            };
        }
    }
}