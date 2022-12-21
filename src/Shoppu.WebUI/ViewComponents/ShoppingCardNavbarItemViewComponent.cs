using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shoppu.Application.ShoppingCart.Queries;

namespace Shoppu.WebUI.ViewComponents
{
    public class ShoppingCardNavbarItemViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public ShoppingCardNavbarItemViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = await _mediator.Send(new GetCartForUserQuery(User));
            return View(cart);
        }
    }
}