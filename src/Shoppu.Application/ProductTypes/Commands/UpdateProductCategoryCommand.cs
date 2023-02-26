using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
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
                .Select(pc => new ProductCategory
                {
                    Id = pc.Id,
                    Name = pc.Name,
                    UrlName = pc.UrlName,
                    ParentCategoryId = pc.ParentCategoryId,
                    ParentCategory = pc.ParentCategory,
                    SpecificGender = pc.SpecificGender,
                    SubCategories = pc.SubCategories,
                    HasNoExistingProducts = !pc.Products.Any()
                })
                .FirstOrDefaultAsync();


            if (categoryFromDb != null)
            {
                if (request.EditedCategory.Name == null && request.EditedCategory.UrlName == null && categoryFromDb.SpecificGender == request.EditedCategory.SpecificGender)
                {
                    return new NotificationMessageViewModel
                    {
                        StatusType = Domain.Enums.StatusType.Info,
                        Message = "No changes to category were made."
                    };
                }

                if (request.EditedCategory.Name != null && !request.EditedCategory.Name.Equals(categoryFromDb.Name))
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

                if (request.EditedCategory.UrlName != null && !request.EditedCategory.UrlName.ToLower().Equals(categoryFromDb.UrlName))
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

                // specific gender was changed
                if (categoryFromDb.SpecificGender != request.EditedCategory.SpecificGender)
                {
                    if (categoryFromDb.ParentCategoryId != null)
                    {
                        var parentCategoryFromDb = await _context.ProductCategories
                            .Where(pc => pc.Id == categoryFromDb.ParentCategoryId)
                            .Select(pc => new ProductCategory
                            {
                                Id = pc.Id,
                                Name = pc.Name,
                                UrlName = pc.UrlName,
                                SpecificGender = pc.SpecificGender,
                                HasNoExistingProducts = !pc.Products.Any()
                            })
                            .FirstOrDefaultAsync();

                        // if parent category is one gender, subcategory cannot be the opposite gender, only the same
                        if (parentCategoryFromDb.SpecificGender != null && !parentCategoryFromDb.SpecificGender.Equals(request.EditedCategory.SpecificGender))
                        {
                            return new NotificationMessageViewModel
                            {
                                StatusType = Domain.Enums.StatusType.Warning,
                                Message = "Specified gender could not be changed for this category."
                            };
                        }

                        if (!categoryFromDb.HasNoExistingProducts)
                        {
                            return new NotificationMessageViewModel
                            {
                                StatusType = Domain.Enums.StatusType.Warning,
                                Message = "Cannot change gender for this category because it already contains products."
                            };
                        }

                        if (categoryFromDb.SubCategories.Count() == 0)
                        {
                            categoryFromDb.SpecificGender = request.EditedCategory.SpecificGender;
                        }

                        else
                        {
                            return new NotificationMessageViewModel
                            {
                                StatusType = Domain.Enums.StatusType.Warning,
                                Message = "Specified gender could not be changed for this category because it already has a subcategory."
                            };
                        }
                    }
                }

                categoryFromDb.Name = request.EditedCategory.Name ?? categoryFromDb.Name;
                categoryFromDb.UrlName = request.EditedCategory.UrlName?.ToLower() ?? categoryFromDb.UrlName;

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