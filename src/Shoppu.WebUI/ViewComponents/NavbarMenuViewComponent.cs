using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoppu.Application.Products.Queries.Shop;
using Shoppu.Domain.Entities;

namespace SimpleShop.Web.ViewComponents
{
    public class NavbarMenuViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public NavbarMenuViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var NavbarMenuData = await _mediator.Send(new GetNavbarMenuCategoriesDataQuery());

            return View(NavbarMenuData);
        }
    }
}