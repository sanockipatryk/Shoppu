using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.Products.Commands.CreateProduct;
using Shoppu.Application.Products.Queries;
using Shoppu.Application.ProductTypes.Queries;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;
using Shoppu.WebUI.Utilities;

namespace Shoppu.WebUI.Pages.Products
{
    public class CreateVariantModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CreateVariantModel(
            IMediator mediator,
            IWebHostEnvironment webHostEnvironment)
        {
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public CreateProductVariantViewModel ProductVariant { get; set; } = new CreateProductVariantViewModel();
        public Product Product { get; set; }
        public List<ProductVariant> ExistingVariants { get; set; }
        public List<Variant> PossibleVariants { get; set; }
        public async Task<IActionResult> OnGet(int id)
        {
            Product = await _mediator.Send(new GetProductQuery(id));
            if (Product.Name != null)
            {
                ExistingVariants = await _mediator.Send(new GetProductVariantListQuery(id));
                PossibleVariants = await _mediator.Send(new GetVariantListQuery());
                return Page();
            }
            return RedirectToPage("Error");
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var newVariant = await _mediator.Send(new CreateProductVariantCommand(ProductVariant));

                var imagePaths = UploadFilesToWebRoot.UploadManyFiles(
                    _webHostEnvironment,
                    HttpContext.Request,
                    "productVariants",
                    newVariant.Product.ProductCategory.Name,
                    newVariant.Product.Id.ToString(),
                    newVariant.Product.Name,
                    newVariant.Id.ToString()
                    );
                if (imagePaths != null)
                {
                    var addImagesResult = await _mediator.Send(new CreateProductVariantImagesCommand(newVariant.Id, imagePaths));

                    if (addImagesResult)
                        return RedirectToPage("Manage");
                }

            }
            Product = await _mediator.Send(new GetProductQuery(ProductVariant.ProductId));
            if (Product.Name != null)
            {
                ExistingVariants = await _mediator.Send(new GetProductVariantListQuery(ProductVariant.ProductId));
                PossibleVariants = await _mediator.Send(new GetVariantListQuery());
            }
            return Page();
        }
    }
}
