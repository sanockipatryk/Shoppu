using System.Security.Principal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.Enums;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.ShoppingCart.Commands
{
    public record RemoveAllItemsFromCartCommand(IPrincipal User, CartItemAction Action) : IRequest<NotificationMessageViewModel>;

    public class RemoveAllItemsFromCartCommandHandler : IRequestHandler<RemoveAllItemsFromCartCommand, NotificationMessageViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public RemoveAllItemsFromCartCommandHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<NotificationMessageViewModel> Handle(RemoveAllItemsFromCartCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.User.Identity.Name);

            var userCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (userCart != null)
            {
                if (request.Action == CartItemAction.RemoveAllItems)
                {
                    var allCartItems = await _context.CartItems
                        .Where(ci => ci.CartId == userCart.Id)
                        .ToListAsync();

                    _context.CartItems.RemoveRange(allCartItems);

                    await _context.SaveChangesAsync(cancellationToken);
                    return new NotificationMessageViewModel
                    {
                        StatusType = StatusType.Success,
                        Message = "Removed all items from your cart."
                    };
                }
            }
            return new NotificationMessageViewModel
            {
                StatusType = StatusType.Danger,
                Message = "Something went wrong. Try again."
            };

        }
    }
}