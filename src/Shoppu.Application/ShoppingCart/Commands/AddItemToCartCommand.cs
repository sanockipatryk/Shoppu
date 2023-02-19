using System.Security.Principal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Application.Helpers;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.ShoppingCart.Commands
{
    public record AddItemToCartCommand(IPrincipal User, int ProductVariantSizeId) : IRequest<NotificationMessageViewModel>;
    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, NotificationMessageViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AddItemToCartCommandHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<NotificationMessageViewModel> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.User.Identity.Name);
            var userCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (userCart != null)
            {
                var cartItem = await _context.CartItems
                .Where(ci => ci.CartId == userCart.Id && ci.ProductVariantSizeId == request.ProductVariantSizeId)
                .Include(ci => ci.ProductVariantSize)
                .FirstOrDefaultAsync();

                // if the same product in the same size already exists in this user cart
                if (cartItem != null)
                {
                    return await CartItemHelpers.UpdateCartItemOnAdd(_context, cartItem, cancellationToken);
                }
                else
                {
                    // if not such item already in cart
                    var selectedItemFromDb = await _context.ProductVariantSizes.FirstOrDefaultAsync(pvs => pvs.Id == request.ProductVariantSizeId);
                    if (selectedItemFromDb.Quantity > 0)
                    {
                        // if selected items quantity is still higher than 0
                        await _context.CartItems.AddAsync(new CartItem
                        {
                            CartId = userCart.Id,
                            ProductVariantSizeId = selectedItemFromDb.Id,
                            Quantity = 1
                        });

                        await _context.SaveChangesAsync(cancellationToken);

                        return new NotificationMessageViewModel
                        {
                            StatusType = Domain.Enums.StatusType.Success,
                            Message = "Product added to cart."
                        };
                    }
                    else
                    {
                        // if products quantity is 0
                        return new NotificationMessageViewModel
                        {
                            StatusType = Domain.Enums.StatusType.Danger,
                            Message = "This size has run out of stock."
                        };
                    }
                }
            }

            return new NotificationMessageViewModel
            {
                StatusType = Domain.Enums.StatusType.Danger,
                Message = "Something went wrong. Try again later."
            };

        }
    }

}