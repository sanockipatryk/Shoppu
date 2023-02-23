using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;

namespace Shoppu.Application.Products.Queries
{
    public record GetProductUrlDataQuery(int ProductId) : IRequest<Product>;

    public class GetProductUrlDataQueryHandler : IRequestHandler<GetProductUrlDataQuery, Product>
    {
        private readonly IApplicationDbContext _context;
        public GetProductUrlDataQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(GetProductUrlDataQuery request, CancellationToken cancellationToken)
        {
            var productData = await _context.Products
            .Where(p => p.Id == request.ProductId)
            .Select(p => new Product
            {
                Id = p.Id,
                Code = p.Code,
                ProductCategory = new ProductCategory
                {
                    UrlName = p.ProductCategory.UrlName
                }
            })
            .FirstOrDefaultAsync();

            return productData;
        }
    }
}