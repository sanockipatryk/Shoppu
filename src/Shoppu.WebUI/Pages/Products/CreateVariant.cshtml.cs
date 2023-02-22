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

        public NotificationMessageViewModel Notification { get; set; }

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
            if (Request.Form.Files.Count == 0)
            {
                ViewData["imagesError"] = "Add at least one image.";
            }
            if (ModelState.IsValid && Request.Form.Files.Count > 0)
            {
                var notificationWithUrlValues = await _mediator.Send(new CreateProductVariantCommand(ProductVariant));

                Notification = notificationWithUrlValues.Notification;

                if (Notification.StatusType == Domain.Enums.StatusType.Success)
                {
                    var imagePaths = UploadFilesToWebRoot.UploadManyFiles(
                        _webHostEnvironment,
                        HttpContext.Request,
                        "productVariants",
                        notificationWithUrlValues.CategoryUrl,
                        notificationWithUrlValues.ProductCode,
                        notificationWithUrlValues.ProductVariantCode
                        );

                    var addImagesResult = await _mediator.Send(new CreateProductVariantImagesCommand(notificationWithUrlValues.ProductVariantId, imagePaths));

                    if (addImagesResult)
                        return RedirectToPage("Manage", new { categoryUrl = notificationWithUrlValues.CategoryUrl, code = notificationWithUrlValues.ProductCode });
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
