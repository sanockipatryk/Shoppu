using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            var SideMenuData = await _mediator.Send(new GetBrowseProductsSideMenuCategoriesQuery(categoryId));

            return View(SideMenuData);
        }
    }
}