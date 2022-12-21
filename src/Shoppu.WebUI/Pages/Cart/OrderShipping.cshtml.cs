using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.ShopOrder.Commands;
using Shoppu.Application.ShoppingCart.Queries;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Cart
{
    public class OrderShipping : PageModel
    {
        private readonly IMediator _mediator;

        public OrderShipping(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public ShipmentDataViewModel ShippmentData { get; set; }
        public ViewCartViewModel Cart { get; set; }

        public NotificationMessageViewModel? Notification { get; set; }

        public async Task OnGet()
        {
            Cart = await _mediator.Send(new GetCartWithCartItemsQuery(User, true));
            Notification = Cart.CartLoadingNotification;
            Cart.IsEditable = false;

            var location = new Uri($"{Request.Scheme}://{Request.Host}");
            ViewData["ImageUrlPrefix"] = location.AbsoluteUri;
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                Notification = await _mediator.Send(new CreateOrderCommand(User, ShippmentData));
                // return RedirectToPage("ViewCart");
                // return Page();
            }

            Cart = await _mediator.Send(new GetCartWithCartItemsQuery(User, false));
            Cart.IsEditable = false;

            var location = new Uri($"{Request.Scheme}://{Request.Host}");
            ViewData["ImageUrlPrefix"] = location.AbsoluteUri;
            return Page();
        }
    }
}