using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.ProductTypes.Commands;
using Shoppu.Application.ProductTypes.Queries;
using Shoppu.Domain.Entities;
using Shoppu.Domain.Enums;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Products
{
    public class ManageCategoriesModel : PageModel
    {
        private readonly IMediator _mediator;
        public ManageCategoriesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<ProductCategory> ProductCategories { get; set; }


        public CreateProductCategoryViewModel NewCategory { get; set; }

        public EditProductCategoryViewModel EditCategory { get; set; }

        public NotificationMessageViewModel Notification { get; set; }

        public ProductCategoryDisplayAction DisplayAction { get; set; } = ProductCategoryDisplayAction.Create;

        public async Task OnGet()
        {
            ProductCategories = await _mediator.Send(new GetProductCategoriesListQuery(true));
            NewCategory = new CreateProductCategoryViewModel();
            EditCategory = new EditProductCategoryViewModel();
        }

        public async Task OnPostCreateCategory(CreateProductCategoryViewModel newCategory)
        {
            if (ModelState.IsValid)
            {
                Notification = await _mediator.Send(new CreateProductCategoryCommand(newCategory));

                if (Notification.StatusType.Equals(StatusType.Success))
                {
                    ModelState.Clear();
                    NewCategory = new CreateProductCategoryViewModel();
                }
                else
                {
                    NewCategory = newCategory;
                }
            }
            else
            {
                NewCategory = newCategory;
            }

            DisplayAction = ProductCategoryDisplayAction.Create;
            ProductCategories = await _mediator.Send(new GetProductCategoriesListQuery(true));
            EditCategory = new EditProductCategoryViewModel();
        }

        public async Task OnPostDeleteCategory(int categoryId)
        {
            if (ModelState.IsValid)
            {
                Notification = await _mediator.Send(new DeleteProductCategoryCommand(categoryId));
            }

            DisplayAction = ProductCategoryDisplayAction.Create;
            ProductCategories = await _mediator.Send(new GetProductCategoriesListQuery(true));
            NewCategory = new CreateProductCategoryViewModel();
            EditCategory = new EditProductCategoryViewModel();

        }

        public async Task OnPostEditCategory(EditProductCategoryViewModel editCategory)
        {
            if (ModelState.IsValid)
            {
                Notification = await _mediator.Send(new UpdateProductCategoryCommand(editCategory));

                if (Notification.StatusType.Equals(StatusType.Info))
                {
                    ModelState.Clear();
                    EditCategory = new EditProductCategoryViewModel();
                    DisplayAction = ProductCategoryDisplayAction.Create;
                }
                else
                {
                    EditCategory = editCategory;
                    DisplayAction = ProductCategoryDisplayAction.Edit;
                }
            }
            else
            {
                EditCategory = editCategory;
                DisplayAction = ProductCategoryDisplayAction.Edit;
            }

            ProductCategories = await _mediator.Send(new GetProductCategoriesListQuery(true));
            NewCategory = new CreateProductCategoryViewModel();
        }
    }
}