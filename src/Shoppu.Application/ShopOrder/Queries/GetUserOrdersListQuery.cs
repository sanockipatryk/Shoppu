using System.Security.Principal;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoppu.Application.Common.Interfaces;
using Shoppu.Domain.Entities;
using Shoppu.Domain.ViewModels;
using Shoppu.Application.Helpers;

namespace Shoppu.Application.ShopOrder.Queries
{
    public record GetUserOrdersListQuery(IPrincipal User, PaginationViewModel Pagination) : IRequest<UserOrderListViewModel>;
    public class GetUserOrdersListQueryHandler : IRequestHandler<GetUserOrdersListQuery, UserOrderListViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetUserOrdersListQueryHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserOrderListViewModel> Handle(GetUserOrdersListQuery request, CancellationToken cancellationToken)
        {
            PaginationViewModel newPagination = new PaginationViewModel
            {
                Page = request.Pagination.Page,
                ItemsPerPage = request.Pagination.ItemsPerPage
            };

            var user = await _userManager.FindByNameAsync(request.User.Identity.Name);

            var userOrders = await _context.OrderDetails
                .Where(od => od.Order.UserId == user.Id)
                .OrderByDescending(od => od.Order.DateOrdered)
                .ApplyPagination(ref newPagination)
                .Select(od => new OrderDetails
                {
                    TotalPrice = od.TotalPrice,
                    PaymentMethod = od.PaymentMethod,
                    Order = new Order
                    {
                        Number = od.Order.Number,
                        UserId = od.Order.UserId,
                        DateOrdered = od.Order.DateOrdered,
                        Items = od.Order.Items.Select(odi => new OrderItem
                        {
                            Quantity = odi.Quantity
                        }).ToList()
                    }
                })
                .ToListAsync();

            if (userOrders.Count() > 0)
            {
                var listOfUserOrders = new List<UserOrderViewModel>();

                foreach (var order in userOrders)
                {
                    listOfUserOrders.Add(new UserOrderViewModel
                    {
                        DateOrdered = order.Order.DateOrdered,
                        NumberOfItems = order.Order.Items.Sum(odi => odi.Quantity),
                        OrderNumber = order.Order.Number,
                        TotalPrice = order.TotalPrice,
                        PaymentMethod = order.PaymentMethod
                    });
                }

                return new UserOrderListViewModel
                {
                    OrderList = listOfUserOrders,
                    Pagination = newPagination
                };
            }
            return null;
        }
    }
}