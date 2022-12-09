using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Queries.Shop
{
    public record GetNavbarMenuCategoriesDataQuery : IRequest<NavbarMenuDataViewModel>;

    public class GetNavbarMenuCategoriesDataQueryHandler : IRequestHandler<GetNavbarMenuCategoriesDataQuery, NavbarMenuDataViewModel>
    {
        private readonly IApplicationDbContext _context;

        public GetNavbarMenuCategoriesDataQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NavbarMenuDataViewModel> Handle(GetNavbarMenuCategoriesDataQuery request, CancellationToken cancellationToken)
        {
            var mainCategoriesWithSubcategories = await _context.ProductCategories
            .Where(pc => pc.ParentCategoryId == null)
            .Include(pc => pc.SubCategories)
            .ToListAsync();

            return new NavbarMenuDataViewModel
            {
                MainCategoriesWithTheirSubcategories = mainCategoriesWithSubcategories
            };
        }
    }
}