using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Queries
{
    public record GetProductVariantForEditQuery(int ProductId, int VariantId) : IRequest<EditProductVariantViewModel>;

    public class GetProductVariantForEditQueryHandler : IRequestHandler<GetProductVariantForEditQuery, EditProductVariantViewModel>
    {
        private readonly IApplicationDbContext _context;

        public GetProductVariantForEditQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EditProductVariantViewModel> Handle(GetProductVariantForEditQuery request, CancellationToken cancellationToken)
        {
            EditProductVariantViewModel productVariantForEdit = await _context.ProductVariants
                .Where(pv => pv.Id == request.VariantId && pv.ProductId == request.ProductId)
                .Select(pv => new EditProductVariantViewModel
                {
                    ProductVariantId = pv.Id,
                    ProductId = pv.ProductId,
                    VariantId = pv.VariantId,
                    Name = pv.Name,
                    Price = pv.Price.ToString() ?? null,
                    IsAccessible = pv.IsAccessible,
                    ProductCode = pv.Code.Substring(0, pv.Code.IndexOf("/")).ToString(),
                    CodeAddition = pv.Code.Substring(pv.Code.IndexOf("/") + 1).ToString(),
                    ImagePaths = pv.Images.Select(pvi => pvi.ImageSource).ToList()
                })
                .FirstOrDefaultAsync();

            return productVariantForEdit;
        }
    }
}