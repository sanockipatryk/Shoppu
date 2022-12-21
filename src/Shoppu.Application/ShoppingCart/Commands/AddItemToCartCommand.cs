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
                    // if (cartItem.Quantity < cartItem.ProductVariantSize.Quantity)
                    // {
                    //     // if quantity of selected item is lower than the overall available quantity of the item
                    //     // add one quantity to the selected item 
                    //     cartItem.Quantity++;
                    //     _context.CartItems.Update(cartItem);
                    //     await _context.SaveChangesAsync(cancellationToken);

                    //     return new NotificationMessageViewModel
                    //     {
                    //         StatusType = Domain.Enums.StatusType.Success,
                    //         Message = "Product added to cart."
                    //     };
                    // }
                    // else if (cartItem.Quantity == cartItem.ProductVariantSize.Quantity)
                    // {
                    //     // if quantity of selected item is the same as available quantity - no change, just information for user
                    //     return new NotificationMessageViewModel
                    //     {
                    //         StatusType = Domain.Enums.StatusType.Info,
                    //         Message = "No more items of this size in stock than what you have in your cart already."
                    //     };
                    // }
                    // else if (cartItem.ProductVariantSize.Quantity > 0 && cartItem.ProductVariantSize.Quantity < cartItem.Quantity)
                    // {
                    //     // if available quantity is more than 0, but less than what the user already has in his cart
                    //     // set cart quantity to available quantity and inform user

                    //     cartItem.Quantity = cartItem.ProductVariantSize.Quantity;
                    //     _context.CartItems.Update(cartItem);
                    //     await _context.SaveChangesAsync(cancellationToken);

                    //     return new NotificationMessageViewModel
                    //     {
                    //         StatusType = Domain.Enums.StatusType.Warning,
                    //         Message = "Availability of product has changed, your cart was updated to match the maximum available number of items."
                    //     };
                    // }
                    // else if (cartItem.ProductVariantSize.Quantity == 0)
                    // {
                    //     _context.CartItems.Remove(cartItem);
                    //     await _context.SaveChangesAsync(cancellationToken);

                    //     return new NotificationMessageViewModel
                    //     {
                    //         StatusType = Domain.Enums.StatusType.Danger,
                    //         Message = "This size has run out of stock, product was removed from your cart."
                    //     };
                    // }
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