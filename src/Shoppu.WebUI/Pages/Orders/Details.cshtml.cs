using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shoppu.Application.ShopOrder.Queries;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Orders
{
    public class DetailsModel : PageModel
    {

        private readonly IMediator _mediator;

        public DetailsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public OrderDetailsViewModel OrderDetails { get; set; }


        public async Task OnGet(int orderNumber)
        {
            OrderDetails = await _mediator.Send(new GetUserOrderDetailsQuery(User, orderNumber));

            var location = new Uri($"{Request.Scheme}://{Request.Host}");
            ViewData["ImageUrlPrefix"] = location.AbsoluteUri;
        }
    }
}