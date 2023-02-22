using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Shoppu.Application.Colors.Commands;
using Shoppu.Application.Colors.Queries;
using Shoppu.Domain.Entities;
using Shoppu.Domain.Enums;
using Shoppu.Domain.ViewModels;

namespace Shoppu.WebUI.Pages.Products
{
    public class ManageColors : PageModel
    {
        private readonly IMediator _mediator;

        public ManageColors(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public CreateColorViewModel NewColorVariant { get; set; }
        public List<Variant> ExistingVariants { get; set; }
        public NotificationMessageViewModel Notification { get; set; }

        public async Task OnGet()
        {
            ExistingVariants = await _mediator.Send(new GetAllColorsQuery());
            NewColorVariant = new CreateColorViewModel();
        }

        public async Task OnPost()
        {
            if (ModelState.IsValid)
            {
                Notification = await _mediator.Send(new CreateColorVariantCommand(NewColorVariant));
                if (Notification.StatusType.Equals(StatusType.Success))
                {
                    ModelState.Clear();
                    NewColorVariant = new CreateColorViewModel();
                }
            }
            ExistingVariants = await _mediator.Send(new GetAllColorsQuery());

        }
    }
}