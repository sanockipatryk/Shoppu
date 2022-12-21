using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.ShoppingCart.Queries
{
    public record GetCartWithCartItemsQuery(IPrincipal User, bool CheckAvailability) : IRequest<ViewCartViewModel>;

    public class GetCartWithCartItemsQueryHandler : IRequestHandler<GetCartWithCartItemsQuery, ViewCartViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetCartWithCartItemsQueryHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ViewCartViewModel> Handle(GetCartWithCartItemsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.User.Identity.Name);

            var userCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == user.Id);

            NotificationMessageViewModel notification = new NotificationMessageViewModel();
            if (request.CheckAvailability)
            {
                var currentUserCartItems = await _context.CartItems
                    .Where(ci => ci.CartId == userCart.Id)
                    .Include(ci => ci.ProductVariantSize)
                    .ToListAsync();

                int itemsChanged = 0;

                foreach (var item in currentUserCartItems)
                {
                    if (item.Quantity > item.ProductVariantSize.Quantity && item.ProductVariantSize.Quantity > 0)
                    {
                        item.Quantity = item.ProductVariantSize.Quantity;
                        _context.CartItems.Update(item);
                        itemsChanged++;

                    }
                    else if (item.Quantity > item.ProductVariantSize.Quantity && item.ProductVariantSize.Quantity == 0)
                    {
                        _context.CartItems.Remove(item);
                        itemsChanged++;
                    }
                }
                if (itemsChanged > 0)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    notification.StatusType = Domain.Enums.StatusType.Warning;
                    notification.Message = "Availability of some items in your cart have changed and your cart was updated. Please review your cart before you continue with the order.";
                }
            }

            var userCartItems = await _context.CartItems
                .Where(ci => ci.CartId == userCart.Id)
                .Select(ci => new CartItem
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    Quantity = ci.Quantity,
                    ProductVariantSize = new ProductVariantSize
                    {
                        Id = ci.ProductVariantSize.Id,
                        Quantity = ci.ProductVariantSize.Quantity,
                        ProductVariant = new ProductVariant
                        {
                            Id = ci.ProductVariantSize.ProductVariant.Id,
                            Name = ci.ProductVariantSize.ProductVariant.Name,
                            IsAccessible = ci.ProductVariantSize.ProductVariant.IsAccessible,
                            Price = ci.ProductVariantSize.ProductVariant.Price,
                            Slug = ci.ProductVariantSize.ProductVariant.Slug,
                            Product = new Product
                            {
                                Id = ci.ProductVariantSize.ProductVariant.Product.Id,
                                Name = ci.ProductVariantSize.ProductVariant.Product.Name,
                                Price = ci.ProductVariantSize.ProductVariant.Product.Price,
                                Gender = ci.ProductVariantSize.ProductVariant.Product.Gender
                            },
                            Images = ci.ProductVariantSize.ProductVariant.Images.Select(cii => new ProductVariantImage
                            {
                                ImageSource = cii.ImageSource
                            }).Take(1).ToList()
                        },
                        Size = new Size
                        {
                            Id = ci.ProductVariantSize.Size.Id,
                            Name = ci.ProductVariantSize.Size.Name
                        }
                    }
                })
                .ToListAsync();

            return new ViewCartViewModel
            {
                CartItems = userCartItems,
                CartLoadingNotification = notification
            };
        }
    }
}