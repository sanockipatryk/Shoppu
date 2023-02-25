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
        public ManageDataViewModel ManageData { get; set; }
        public string CategoryUrl { get; set; }

        public async Task OnGet(string? categoryUrl, ManageProductsFiltersViewModel? filters, int? currentPage)
        {
            PaginationViewModel pagination = new PaginationViewModel
            {
                Page = currentPage ?? 1,
                ItemsPerPage = (int)filters.ItemsPerPage
            };

            CategoryUrl = categoryUrl;
            ManageData = await _mediator.Send(new GetProductsListQuery(CategoryUrl, filters, pagination));
        }

        public async Task OnPostSetAccessible(string? categoryUrl, int variantId)
        {
            var productCode = await _mediator.Send(new SetProductVariantAccessibleCommand(variantId));
            CategoryUrl = categoryUrl;

            var newFilters = new ManageProductsFiltersViewModel
            {
                Code = productCode
            };

            PaginationViewModel pagination = new PaginationViewModel
            {
                Page = 1,
                ItemsPerPage = (int)newFilters.ItemsPerPage
            };

            ManageData = await _mediator.Send(new GetProductsListQuery(CategoryUrl, newFilters, pagination));
        }
    }
}
