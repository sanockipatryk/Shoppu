using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.Products.Queries;
using Shoppu.Application.Sizes.Commads;
using Shoppu.Application.Sizes.Queries;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Products
{
    public class AddVariantSizesModel : PageModel
    {
        private readonly IMediator _mediator;
        public AddVariantSizesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public List<AddVariantSizesViewModel> AddVariantSizes { get; set; }
        [BindProperty]
        public int ProductId { get; set; }
        [BindProperty]
        public int VariantId { get; set; }

        public Product Product { get; set; }

        public NotificationMessageViewModel? Notification { get; set; }

        public async Task OnGet(int productId, int variantId)
        {
            ProductId = productId;
            VariantId = variantId;
            Product = await _mediator.Send(new GetProductUrlDataQuery(ProductId));
            AddVariantSizes = await _mediator.Send(new GetExistingSizesAndPossibleSizesForProductQuery(variantId));
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var notificationWithUrlValues = await _mediator.Send(new AddExistingSizesAndPossibleSizesForProductCommand(VariantId, AddVariantSizes));

                Notification = notificationWithUrlValues.Notification;

                if (Notification.StatusType == Domain.Enums.StatusType.Success)
                {
                    return RedirectToPage("Manage", new { categoryUrl = notificationWithUrlValues.CategoryUrl, code = notificationWithUrlValues.ProductCode });
                }
            }

            Product = await _mediator.Send(new GetProductUrlDataQuery(ProductId));
            AddVariantSizes = await _mediator.Send(new GetExistingSizesAndPossibleSizesForProductQuery(VariantId));
            return Page();
        }
    }
}