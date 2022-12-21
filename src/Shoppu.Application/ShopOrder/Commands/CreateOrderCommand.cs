using System.Security.Principal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.SSOT;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.ShopOrder.Commands
{
    public record CreateOrderCommand(IPrincipal User, ShipmentDataViewModel ShipmentData) : IRequest<NotificationMessageViewModel>;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, NotificationMessageViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CreateOrderCommandHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<NotificationMessageViewModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.User.Identity.Name);

            var userCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == user.Id);

            var currentCartItems = await _context.CartItems
                    .Where(ci => ci.CartId == userCart.Id)
                    .Include(ci => ci.ProductVariantSize)
                    .ThenInclude(pvs => pvs.ProductVariant)
                .ThenInclude(pv => pv.Product)
                    .ToListAsync();

            if (currentCartItems.Count > 0)
            {
                int itemsChanged = 0;

                foreach (var item in currentCartItems)
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
                    return new NotificationMessageViewModel
                    {
                        StatusType = Domain.Enums.StatusType.Warning,
                        Message = "Not enough products in stock to fill your order. Please review changes to items in your cart before you continue."
                    };
                }
                else
                {

                    var totalItemsPrice = currentCartItems.Sum(ci => (ci.ProductVariantSize.ProductVariant.Price ?? ci.ProductVariantSize.ProductVariant.Product.Price) * ci.Quantity);
                    var totalPrice = totalItemsPrice + Defaults.DefaultShipmentCost;

                    var orderItemsList = new List<OrderItem>();
                    foreach (var cartItem in currentCartItems)
                    {
                        orderItemsList.Add(new OrderItem
                        {
                            ProductVariantSizeId = cartItem.ProductVariantSizeId,
                            Quantity = cartItem.Quantity,
                        });
                    }

                    int lowestOrderNumber = Int32.MaxValue;
                    if (await _context.Orders.AnyAsync())
                    {
                        var lowestOrderNumberFromDb = await _context.Orders.MinAsync(o => o.Number);
                        if (lowestOrderNumberFromDb < lowestOrderNumber)
                        {
                            lowestOrderNumber = lowestOrderNumberFromDb;
                        }
                    }

                    var newOrderDetails = new OrderDetails
                    {
                        FirstName = request.ShipmentData.FirstName,
                        LastName = request.ShipmentData.LastName,
                        PostalCode = request.ShipmentData.PostalCode,
                        City = request.ShipmentData.City,
                        Street = request.ShipmentData.Street,
                        Apartment = request.ShipmentData.Apartment,
                        Phone = request.ShipmentData.Phone,
                        Email = request.ShipmentData.Email,
                        PaymentMethod = request.ShipmentData.PaymentMethod,
                        TotalPrice = totalPrice,
                        Order = new Order
                        {
                            Id = Guid.NewGuid().ToString(),
                            DateOrdered = DateTime.UtcNow,
                            Number = lowestOrderNumber - 1,
                            User = user,
                            Items = orderItemsList
                        }
                    };
                    await _context.AddRangeAsync(newOrderDetails);

                    foreach (var item in currentCartItems)
                    {
                        item.ProductVariantSize.Quantity -= item.Quantity;
                    }

                    _context.UpdateRange(currentCartItems);
                    _context.CartItems.RemoveRange(currentCartItems);

                    await _context.SaveChangesAsync(cancellationToken);

                    return new NotificationMessageViewModel
                    {
                        StatusType = Domain.Enums.StatusType.Success,
                        Message = "Order has been placed. Thank you for shopping!"
                    };

                }
            }
            else
            {
                return new NotificationMessageViewModel
                {
                    StatusType = Domain.Enums.StatusType.Danger,
                    Message = "Your cart is empty."
                };
            }
        }
    }
}