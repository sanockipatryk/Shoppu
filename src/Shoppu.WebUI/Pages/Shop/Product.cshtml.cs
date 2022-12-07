using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.Products.Queries.Shop;
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

        public ShopProductViewModel ShopProduct { get; set; }

        public async Task OnGet(string slug)
        {
            ShopProduct = await _mediator.Send(new GetShopProductQuery(slug));
        }
    }
}