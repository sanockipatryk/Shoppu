using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.Sizes.Commads;
using Shoppu.Application.Sizes.Queries;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Products
{
    public class CreateVariantSizesModel : PageModel
    {
        private readonly IMediator _mediator;
        public CreateVariantSizesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public List<CreateProductVariantSizeViewModel> ProductVariantSizes { get; set; }
        [BindProperty]
        public int ProductId { get; set; }
        [BindProperty]
        public int VariantId { get; set; }
        public async Task OnGet(int productId, int variantId)
        {
            ProductId = productId;
            VariantId = variantId;
            await LoadPossibleVariantSizes(productId);
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new CreateProductVariantSizesCommand(VariantId, ProductVariantSizes));

                if (result)
                    return RedirectToPage("Manage", new {categoryUrl = "clothes"});
            }
            return Page();
        }

        private async Task LoadPossibleVariantSizes(int productId)
        {
            ProductVariantSizes = new List<CreateProductVariantSizeViewModel>();
            var possibleSizes = await _mediator.Send(new GetPossibleSizesForProductQuery(productId));
            foreach (var size in possibleSizes)
            {
                ProductVariantSizes.Add(new CreateProductVariantSizeViewModel
                {
                    SizeId = size.Id,
                    SizeName = size.Name
                });
            }
        }
    }
}
