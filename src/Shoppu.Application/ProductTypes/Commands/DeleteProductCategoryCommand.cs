using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoppu.Domain.ViewModels;
using MediatR;
using Shoppu.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.ProductTypes.Commands
{
    public record DeleteProductCategoryCommand(int CategoryId) : IRequest<NotificationMessageViewModel>;

    public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, NotificationMessageViewModel>
    {

        private readonly IApplicationDbContext _context;
        public DeleteProductCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationMessageViewModel> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryToDelete = await _context.ProductCategories
                .Where(pc => pc.Id == request.CategoryId)
                .Select(pc => new ProductCategory
                {
                    Id = pc.Id,
                    Name = pc.Name,
                    UrlName = pc.UrlName,
                    ParentCategoryId = pc.ParentCategoryId,
                    HasNoExistingProducts = !pc.Products.Any()
                }).FirstOrDefaultAsync();

            if (categoryToDelete.HasNoExistingProducts)
            {
                var categoryName = categoryToDelete.Name;
                _context.ProductCategories.Remove(categoryToDelete);

                await _context.SaveChangesAsync(cancellationToken);

                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Success,
                    Message = $"Category '{categoryName}' was deleted."
                };
            }

            return new NotificationMessageViewModel
            {
                StatusType = Domain.Enums.StatusType.Warning,
                Message = "Selected category could not be removed."
            };

        }
    }
}