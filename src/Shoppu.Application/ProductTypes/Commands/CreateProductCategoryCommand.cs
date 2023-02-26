using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.ProductTypes.Commands
{
    public record CreateProductCategoryCommand(CreateProductCategoryViewModel NewCategory) : IRequest<NotificationMessageViewModel>;

    public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, NotificationMessageViewModel>
    {
        private readonly IApplicationDbContext _context;
        public CreateProductCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationMessageViewModel> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var parentCategoryFromDb = await _context.ProductCategories
             .Where(pc => pc.Id == request.NewCategory.ParentCategoryId)
             .Select(pc => new ProductCategory
             {
                 Id = pc.Id,
                 Name = pc.Name,
                 HasNoExistingProducts = !pc.Products.Any(),
                 SpecificGender = pc.SpecificGender
             })
             .FirstOrDefaultAsync();

            if (!parentCategoryFromDb.HasNoExistingProducts)
            {
                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Danger,
                    Message = $"Cannot add a subcategory to {parentCategoryFromDb.Name} because it already has products."
                };
            }

            bool categoryExistsWithSameName = await _context.ProductCategories.AnyAsync(pc => pc.Name.ToLower().Equals(request.NewCategory.Name.ToLower()));

            if (categoryExistsWithSameName)
            {
                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Danger,
                    Message = "Category with the same Name already exists."
                };
            }

            bool categoryExistsWithSameUrlName = await _context.ProductCategories.AnyAsync(pc => pc.UrlName.ToLower().Equals(request.NewCategory.UrlName.ToLower()));

            if (categoryExistsWithSameUrlName)
            {
                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Danger,
                    Message = "Category with the same Url Name already exists."
                };
            }

            if (parentCategoryFromDb.SpecificGender != null && !parentCategoryFromDb.SpecificGender.Equals(request.NewCategory.SpecificGender))
            {
                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Warning,
                    Message = $"Subcategory with this gender could not be added."
                };
            }

            await _context.ProductCategories.AddAsync(new ProductCategory
            {
                Name = request.NewCategory.Name,
                UrlName = request.NewCategory.UrlName.ToLower(),
                ParentCategoryId = request.NewCategory.ParentCategoryId,
                SpecificGender = request.NewCategory.SpecificGender
            });

            await _context.SaveChangesAsync(cancellationToken);

            return new NotificationMessageViewModel
            {
                StatusType = Domain.Enums.StatusType.Success,
                Message = $"Category {request.NewCategory.Name} was added."
            };
        }
    }
}