using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.Products.Commands.ManageProduct;
using Shoppu.Application.Products.Queries;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

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
        public ManageProductsFiltersViewModel? AppliedFilters { get; set; }
        public string CategoryUrl { get; set; }

        public async Task OnGet(string? categoryUrl, ManageProductsFiltersViewModel filters)
        {
            CategoryUrl = categoryUrl;
            AppliedFilters = filters;
            Products = await _mediator.Send(new GetProductsListQuery(CategoryUrl, AppliedFilters));
        }

        public async Task OnPostChangeVariantVisibility(string? categoryUrl, int variantId)
        {
            var setAsAccessible = await _mediator.Send(new SetProductVariantAccessibleCommand(variantId));
            CategoryUrl = categoryUrl;
            AppliedFilters = new ManageProductsFiltersViewModel();
            Products = await _mediator.Send(new GetProductsListQuery(CategoryUrl, AppliedFilters));
        }
    }
}
