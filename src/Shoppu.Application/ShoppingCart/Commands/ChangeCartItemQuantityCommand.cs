using System.Security.Principal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Application.Helpers;
using Shoppu.Domain.Entities;
using Shoppu.Domain.Enums;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.ShoppingCart.Commands
{
    public record ChangeCartItemQuantityCommand(IPrincipal User, int ProductVariantSizeId, CartItemAction Action) : IRequest<NotificationMessageViewModel>;
    public class ChangeCartItemQuantityCommandHandler : IRequestHandler<ChangeCartItemQuantityCommand, NotificationMessageViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ChangeCartItemQuantityCommandHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<NotificationMessageViewModel> Handle(ChangeCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.User.Identity.Name);

            var userCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (userCart != null)
            {
                var cartItem = await _context.CartItems
                .Where(ci => ci.CartId == userCart.Id && ci.ProductVariantSizeId == request.ProductVariantSizeId)
                .Include(ci => ci.ProductVariantSize)
                .FirstOrDefaultAsync();

                if (cartItem != null)
                {
                    switch (request.Action)
                    {
                        case CartItemAction.AddOneItem:
                            return await CartItemHelpers.UpdateCartItemOnAdd(_context, cartItem, cancellationToken);

                        case CartItemAction.RemoveOneItem:
                            return await CartItemHelpers.UpdateCartItemOnRemoveOne(_context, cartItem, cancellationToken);

                        case CartItemAction.RemoveManyItems:
                            _context.CartItems.Remove(cartItem);
                            await _context.SaveChangesAsync(cancellationToken);

                            return new NotificationMessageViewModel
                            {
                                StatusType = Domain.Enums.StatusType.Success,
                                Message = "Product removed from cart."
                            };
                    }

                }
                return new NotificationMessageViewModel
                {
                    StatusType = StatusType.Info,
                    Message = "Your cart is empty."
                };
            }

            return new NotificationMessageViewModel
            {
                StatusType = Domain.Enums.StatusType.Danger,
                Message = "Something went wrong"
            };

        }
    }
}