using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Sizes.Queries
{
    public record GetExistingSizesAndPossibleSizesForProductQuery(int VariantId) : IRequest<List<AddVariantSizesViewModel>>;

    public class GetExistingSizesAndPossibleSizesForProductQueryHandler : IRequestHandler<GetExistingSizesAndPossibleSizesForProductQuery, List<AddVariantSizesViewModel>>
    {
        private readonly IApplicationDbContext _context;
        public GetExistingSizesAndPossibleSizesForProductQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AddVariantSizesViewModel>> Handle(GetExistingSizesAndPossibleSizesForProductQuery request, CancellationToken cancellationToken)
        {
            var productVariantFromDb = await _context.ProductVariants
                .Where(pv => pv.Id == request.VariantId)
                .Include(pv => pv.Product)
                .Include(pv => pv.Sizes)
                .ThenInclude(pvs => pvs.Size)
                .FirstOrDefaultAsync();

            var existingSizesIds = productVariantFromDb.Sizes.Select(pvs => pvs.SizeId).ToList();

            var possibleSizesFromDb = await _context.Sizes
                .Where(c => c.SizeTypeId == productVariantFromDb.Product.SizeTypeId)
                .Where(s => !existingSizesIds.Contains(s.Id))
                .ToListAsync();


            var addVariantSizes = new List<AddVariantSizesViewModel>();

            foreach (var existingSize in productVariantFromDb.Sizes)
            {
                addVariantSizes.Add(new AddVariantSizesViewModel
                {
                    Quantity = existingSize.Quantity,
                    QuantityToAdd = 0,
                    SizeId = existingSize.Size.Id,
                    SizeName = existingSize.Size.Name
                });
            }

            foreach (var possibleSize in possibleSizesFromDb)
            {
                addVariantSizes.Add(new AddVariantSizesViewModel
                {
                    AlreadyExists = false,
                    SizeId = possibleSize.Id,
                    SizeName = possibleSize.Name
                });
            }

            return addVariantSizes;
        }
    }
}