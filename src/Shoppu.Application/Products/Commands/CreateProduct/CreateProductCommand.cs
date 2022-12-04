﻿using MediatR;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.Enums;
using Shoppu.Domain.ViewModels;
using System.Globalization;

namespace Shoppu.Application.Products.Commands.CreateProduct
{
    public record CreateProductCommand(CreateProductViewModel ProductViewModel) : IRequest<Product>;

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IApplicationDbContext _context;
        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            var product = new Product
            {
                Name = request.ProductViewModel.Name,
                Price = Decimal.Parse(request.ProductViewModel.Price, CultureInfo.InvariantCulture),
                Description = request.ProductViewModel.Description,
                Gender = (ProductGender) request.ProductViewModel.Gender,
                ProductCategoryId = (int) request.ProductViewModel.ProductCategoryId
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
