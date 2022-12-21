using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Helpers
{
    public static class CartItemHelpers
    {
        public async static Task<NotificationMessageViewModel> UpdateCartItemOnAdd(IApplicationDbContext context, CartItem cartItem, CancellationToken cancellationToken)
        {
            if (cartItem.Quantity < cartItem.ProductVariantSize.Quantity)
            {
                // if quantity of selected item is lower than the overall available quantity of the item
                // add one quantity to the selected item 
                cartItem.Quantity++;
                context.CartItems.Update(cartItem);
                await context.SaveChangesAsync(cancellationToken);

                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Success,
                    Message = "Product added to cart."
                };
            }
            else if (cartItem.Quantity == cartItem.ProductVariantSize.Quantity)
            {
                // if quantity of selected item is the same as available quantity - no change, just information for user
                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Info,
                    Message = "No more items of this size in stock than what you have in your cart already."
                };
            }
            else if (cartItem.ProductVariantSize.Quantity > 0 && cartItem.ProductVariantSize.Quantity < cartItem.Quantity)
            {
                // if available quantity is more than 0, but less than what the user already has in his cart
                // set cart quantity to available quantity and inform user

                cartItem.Quantity = cartItem.ProductVariantSize.Quantity;
                context.CartItems.Update(cartItem);
                await context.SaveChangesAsync(cancellationToken);

                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Warning,
                    Message = "Availability of size of this product has changed, your cart was updated to match the maximum available number of items."
                };
            }
            else if (cartItem.ProductVariantSize.Quantity == 0)
            {
                context.CartItems.Remove(cartItem);
                await context.SaveChangesAsync(cancellationToken);

                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Danger,
                    Message = "This size of the product has run out of stock, product was removed from your cart."
                };
            }

            return new NotificationMessageViewModel
            {
                StatusType = Domain.Enums.StatusType.Danger,
                Message = "Something went wrong"
            };

        }

        public async static Task<NotificationMessageViewModel> UpdateCartItemOnRemoveOne(IApplicationDbContext context, CartItem cartItem, CancellationToken cancellationToken)
        {
            if (cartItem.Quantity >= 2 && cartItem.ProductVariantSize.Quantity >= cartItem.Quantity - 1)
            {
                cartItem.Quantity--;
                context.CartItems.Update(cartItem);
                await context.SaveChangesAsync(cancellationToken);

                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Success,
                    Message = "Removed one of selected products."
                };

            }
            else if (cartItem.Quantity >= 2 && cartItem.ProductVariantSize.Quantity < cartItem.Quantity - 1 && cartItem.ProductVariantSize.Quantity != 0)
            {
                cartItem.Quantity = cartItem.ProductVariantSize.Quantity;
                context.CartItems.Update(cartItem);
                await context.SaveChangesAsync(cancellationToken);

                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Warning,
                    Message = "Availability of size of this product has changed, your cart was updated to match the maximum available number of items."
                };
            }
            else if (cartItem.Quantity >= 2 && cartItem.ProductVariantSize.Quantity == 0)
            {
                context.CartItems.Remove(cartItem);
                await context.SaveChangesAsync(cancellationToken);

                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Danger,
                    Message = "This size of the product has run out of stock, product was removed from your cart."
                };
            }
            else if (cartItem.Quantity == 1)
            {
                context.CartItems.Remove(cartItem);
                await context.SaveChangesAsync(cancellationToken);

                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Success,
                    Message = "Product removed from cart."
                };
            }
            else
            {
                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Danger,
                    Message = "Something went wrong"
                };
            }
        }

    }
}