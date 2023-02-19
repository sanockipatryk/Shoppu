using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shoppu.Application.Products.Queries.Shop;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Shop
{
    public class Latest : PageModel
    {
        private readonly IMediator _mediator;

        public Latest(IMediator mediator)
        {
            _mediator = mediator;

        }

        public BrowseLatestProductsViewModel LatestProducts { get; set; }

        public async Task OnGet()
        {
            LatestProducts = await _mediator.Send(new GetLatestProductsQuery());
        }
    }
}