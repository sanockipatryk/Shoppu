using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.ShopOrder.Queries;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Orders
{
    public class ListModel : PageModel
    {
        private readonly IMediator _mediator;
        public ListModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public UserOrderListViewModel Orders { get; set; }

        public async Task OnGet(int? currentPage)
        {
            PaginationViewModel pagination = new PaginationViewModel();
            pagination.Page = currentPage ?? 1;
            pagination.ItemsPerPage = 10;
            Orders = await _mediator.Send(new GetUserOrdersListQuery(User, pagination));
        }
    }
}