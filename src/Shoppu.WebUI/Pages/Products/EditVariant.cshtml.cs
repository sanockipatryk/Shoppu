using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shoppu.Application.Products.Commands.CreateProduct;
using Shoppu.Application.Products.Commands.EditProduct;
using Shoppu.Application.Products.Queries;
using Shoppu.Application.ProductTypes.Queries;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;
using Shoppu.WebUI.Utilities;

namespace Shoppu.WebUI.Pages.Products
{
    public class EditVariantModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EditVariantModel(
            IMediator mediator,
            IWebHostEnvironment webHostEnvironment)
        {
            _mediator = mediator;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public EditProductVariantViewModel ProductVariant { get; set; }
        public List<ProductVariant> ExistingVariants { get; set; }
        public List<Variant> PossibleVariants { get; set; }

        public async Task<IActionResult> OnGet(int productId, int variantId)
        {
            ProductVariant = await _mediator.Send(new GetProductVariantForEditQuery(productId, variantId));
            if (!ProductVariant.IsAccessible)
            {
                ExistingVariants = await _mediator.Send(new GetProductVariantListQuery(productId));
                PossibleVariants = await _mediator.Send(new GetVariantListQuery());
                return Page();
            }
            return RedirectToPage("Manage", new { categoryUrl = "clothes", code = ProductVariant.ProductCode });
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var editedVariant = await _mediator.Send(new EditProductVariantCommand(ProductVariant));
                if (Request.Form.Files.Count > 0)
                {
                    var existingProductVariantImagePath = await _mediator.Send(new GetSingleProductVariantImagePathQuery(ProductVariant.ProductVariantId));
                    if (existingProductVariantImagePath != null)
                    {
                        UploadFilesToWebRoot.RemoveOldPath(_webHostEnvironment, existingProductVariantImagePath);
                    }

                    var imagePaths = UploadFilesToWebRoot.UploadManyFiles(
                        _webHostEnvironment,
                        HttpContext.Request,
                        "productVariants",
                        editedVariant.Product.ProductCategory.Name,
                        editedVariant.Code.ToString()
                        );
                    var addImagesResult = await _mediator.Send(new CreateProductVariantImagesCommand(editedVariant.Id, imagePaths));
                }
                return RedirectToPage("Manage", new { categoryUrl = "clothes" });
                // return RedirectToPage("Manage", new { categoryUrl = newVariant.Product.ProductCategory.UrlName });
            }

            ExistingVariants = await _mediator.Send(new GetProductVariantListQuery(ProductVariant.ProductId));
            PossibleVariants = await _mediator.Send(new GetVariantListQuery());
            return Page();
        }
    }
}