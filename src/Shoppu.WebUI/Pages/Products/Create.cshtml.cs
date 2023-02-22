using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.Products.Commands.CreateProduct;
using Shoppu.Application.ProductTypes.Queries;
using Shoppu.Application.Sizes.Queries;
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
        public List<SizeType> PossibleSizeTypes { get; set; }

        public NotificationMessageViewModel Notification { get; set; }

        public async Task OnGet()
        {
            Product = new CreateProductViewModel();
            ProductCategories = await _mediator.Send(new GetProductCategoriesListQuery());
            PossibleSizeTypes = await _mediator.Send(new GetAllSizeTypesQuery());
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var notificationWithUrlValues = await _mediator.Send(new CreateProductCommand(Product));
                if (notificationWithUrlValues.Notification.StatusType == Domain.Enums.StatusType.Success)
                {
                    return RedirectToPage("Manage", new
                    {
                        categoryUrl = notificationWithUrlValues.CategoryUrl,
                        code = notificationWithUrlValues.ProductCode
                    });
                }
                else
                {
                    Notification = notificationWithUrlValues.Notification;
                }
            }
            ProductCategories = await _mediator.Send(new GetProductCategoriesListQuery());
            PossibleSizeTypes = await _mediator.Send(new GetAllSizeTypesQuery());
            return Page();
        }
    }
}
