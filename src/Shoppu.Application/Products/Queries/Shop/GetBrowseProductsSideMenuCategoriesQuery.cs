using MediatR;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Products.Queries.Shop
{
    public record GetBrowseProductsSideMenuCategoriesQuery(int CurrentProductCategoryId) : IRequest<BrowseSideMenuViewModel>;

    public class GetBrowseProductsSideMenuCategoriesQueryHandler : IRequestHandler<GetBrowseProductsSideMenuCategoriesQuery, BrowseSideMenuViewModel>
    {
        private readonly IApplicationDbContext _context;

        public GetBrowseProductsSideMenuCategoriesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BrowseSideMenuViewModel> Handle(GetBrowseProductsSideMenuCategoriesQuery request, CancellationToken cancellationToken)
        {
            var productCategories = await _context.ProductCategories
                .Include(pc => pc.ParentCategory)
                .ToListAsync();

            return new BrowseSideMenuViewModel
            {
                ProductCategories = productCategories,
                CurrentProductCategoryId = request.CurrentProductCategoryId
            };
        }
    }
}