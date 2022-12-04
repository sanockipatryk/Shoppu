using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.Products.Queries;
using Shoppu.Domain.Entities;

namespace Shoppu.WebUI.Pages.Products
{
    public class ManageModel : PageModel
    {
        private readonly IMediator _mediator;
        public ManageModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<Product> Products { get; set; }

        public async Task OnGet()
        {
            Products = await _mediator.Send(new GetProductsListQuery());
        }
    }
}
