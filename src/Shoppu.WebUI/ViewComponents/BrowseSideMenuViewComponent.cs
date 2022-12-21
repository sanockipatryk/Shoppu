using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shoppu.Application.Products.Queries.Shop;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.ViewComponents
{
    public class BrowseSideMenuViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public BrowseSideMenuViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryUrl, ManageProductsFiltersViewModel? appliedManageFilters)
        {
            var SideMenuData = await _mediator.Send(new GetBrowseProductsSideMenuCategoriesQuery(categoryUrl));
            SideMenuData.ManageProductsFilters = appliedManageFilters;
            var x = ViewContext.RouteData.Values["page"];

            return View(SideMenuData);
        }
    }
}