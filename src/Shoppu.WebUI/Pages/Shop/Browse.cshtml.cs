using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shoppu.Application.Products.Queries.Shop;
using Shoppu.Application.ProductTypes.Queries;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Shop
{
    public class BrowseModel : PageModel
    {
        private readonly IMediator _mediator;
        public BrowseModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string Gender;
        public string CategoryUrl;
        public int? CategoryId;

        public BrowseDataViewModel BrowseData { get; set; }


        public async Task OnGet(string gender, string query, string categoryUrl, BrowseProductsFiltersViewModel? filters)
        {
            Gender = gender;
            if (gender.ToLower().Equals("man") || gender.ToLower().Equals("woman"))
            {
                CategoryUrl = categoryUrl;
                BrowseData = await _mediator.Send(new GetBrowseProductsListQuery(categoryUrl, gender, filters));
                BrowseData.Filters = filters;
            }

        }
    }
}