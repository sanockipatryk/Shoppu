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
        public string CategoryName;
        public int? CategoryId;

        public BrowseProductViewModel BrowseProducts { get; set; }

        // public List<BrowseProductViewModel>

        public async Task OnGet(string gender, string query, string categoryName)
        {
            Gender = gender;
            if (gender.ToLower().Equals("male") || gender.ToLower().Equals("female"))
            {
                CategoryName = categoryName;
                CategoryId = await _mediator.Send(new GetProductCategoryIdQuery(categoryName));

                if (CategoryId != null)
                {
                    BrowseProducts = await _mediator.Send(new GetBrowseProductsListQuery((int)CategoryId));
                }
            }

        }
    }
}