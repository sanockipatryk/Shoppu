
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.ShoppingCart.Commands;
using Shoppu.Application.ShoppingCart.Queries;
using Shoppu.Domain.Enums;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Cart
{
    public class ViewCartModel : PageModel
    {
        private readonly IMediator _mediator;

        public ViewCartModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ViewCartViewModel Cart { get; set; }

        public NotificationMessageViewModel? Notification { get; set; }

        public async Task OnGet()
        {
            Cart = await _mediator.Send(new GetCartWithCartItemsQuery(User, true));
            Notification = Cart.CartLoadingNotification;
            Cart.IsEditable = true;

            var location = new Uri($"{Request.Scheme}://{Request.Host}");
            ViewData["ImageUrlPrefix"] = location.AbsoluteUri;
        }

        public async Task OnPostUpdate(int productVariantSizeId, CartItemAction action)
        {
            Notification = await _mediator.Send(new ChangeCartItemQuantityCommand(User, productVariantSizeId, action));
            Cart = await _mediator.Send(new GetCartWithCartItemsQuery(User, false));
            Cart.IsEditable = true;
        }

        public async Task OnPostEmpty(CartItemAction action)
        {
            Notification = await _mediator.Send(new RemoveAllItemsFromCartCommand(User, action));
            Cart = await _mediator.Send(new GetCartWithCartItemsQuery(User, false));
            Cart.IsEditable = true;
        }
    }
}