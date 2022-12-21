using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.Products.Queries.Shop;
using Shoppu.Application.ShoppingCart.Commands;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Shop
{
    public class Product : PageModel
    {
        private readonly IMediator _mediator;

        public Product(IMediator mediator)
        {
            _mediator = mediator;
        }
        public string Slug { get; set; }
        public ShopProductViewModel ShopProduct { get; set; }
        public NotificationMessageViewModel? Notification { get; set; }

        [BindProperty]
        public int ProductVariantSizeId { get; set; }

        public async Task OnGet(string slug)
        {
            ShopProduct = await _mediator.Send(new GetShopProductQuery(slug));
            Slug = slug;
        }

        public async Task OnPost(string slug)
        {
            Notification = await _mediator.Send(new AddItemToCartCommand(User, ProductVariantSizeId));
            ShopProduct = await _mediator.Send(new GetShopProductQuery(slug));
            Slug = slug;
        }
    }
}