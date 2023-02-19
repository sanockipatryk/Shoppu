using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.Products.Commands.CreateProduct;
using Shoppu.Application.ProductTypes.Queries;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;
        public CreateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public CreateProductViewModel Product { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

        public async Task OnGet()
        {
            Product = new CreateProductViewModel();
            ProductCategories = await _mediator.Send(new GetProductCategoriesListQuery());
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var addedProduct = await _mediator.Send(new CreateProductCommand(Product));
                return RedirectToPage("Manage", new { categoryUrl = addedProduct.ProductCategory.UrlName, code = addedProduct.Code });
            }
            ProductCategories = await _mediator.Send(new GetProductCategoriesListQuery());
            return Page();
        }
    }
}
