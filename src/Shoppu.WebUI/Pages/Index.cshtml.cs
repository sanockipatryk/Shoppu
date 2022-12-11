using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoppu.Application.Products.Queries;
using Shoppu.Domain.Entities;

namespace Shoppu.WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;

        public IndexModel(
            ILogger<IndexModel> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task OnGet()
        {
        }
    }
}