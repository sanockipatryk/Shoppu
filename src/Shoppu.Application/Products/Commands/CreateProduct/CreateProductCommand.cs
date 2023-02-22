using MediatR;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.Enums;
using Shoppu.Domain.ViewModels;
using Shoppu.Domain.Extensions;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace Shoppu.Application.Products.Commands.CreateProduct
{
    public record CreateProductCommand(CreateProductViewModel ProductViewModel) : IRequest<NotificationWithUrlData>;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, NotificationWithUrlData>
    {
        private readonly IApplicationDbContext _context;
        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationWithUrlData> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productName = request.ProductViewModel.Name;
            string baseSlug = productName.Slugify();

            do
            {
                string newSlug = baseSlug;
                newSlug = newSlug.AppendRandomDigits(4);
                if (!await _context.Products.AnyAsync(p => p.BaseSlug.Equals(newSlug)))
                {
                    baseSlug = newSlug;
                    break;
                }
            } while (true);

            var productWithExactCodeExists = await _context.Products
                    .AnyAsync(p => p.Code.ToLower()
                    .Equals(request.ProductViewModel.Code.ToLower()));

            if (productWithExactCodeExists)
            {
                return new NotificationWithUrlData
                {
                    Notification = new NotificationMessageViewModel
                    {
                        StatusType = StatusType.Danger,
                        Message = "Product with the same Code already exists."
                    }
                };
            }

            var product = new Product
            {
                Name = request.ProductViewModel.Name,
                Price = Decimal.Parse(request.ProductViewModel.Price, CultureInfo.InvariantCulture),
                Description = request.ProductViewModel.Description,
                Gender = (ProductGender)request.ProductViewModel.Gender,
                ProductCategoryId = (int)request.ProductViewModel.ProductCategoryId,
                Code = request.ProductViewModel.Code.ToUpper(),
                BaseSlug = baseSlug,
                IsAccessible = false,
                SizeTypeId = (int)request.ProductViewModel.SizeTypeId
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync(cancellationToken);

            var addedProduct = await _context.Products
                .Where(p => p.Id == product.Id)
                .Select(p => new Product
                {
                    ProductCategory = new ProductCategory
                    {
                        Id = p.ProductCategory.Id,
                        UrlName = p.ProductCategory.UrlName
                    },
                    Code = p.Code,
                })
                .FirstOrDefaultAsync();

            return new NotificationWithUrlData
            {
                Notification = new NotificationMessageViewModel
                {
                    StatusType = StatusType.Success,
                },
                CategoryUrl = addedProduct.ProductCategory.UrlName,
                ProductCode = addedProduct.Code
            };
        }
    }
}
