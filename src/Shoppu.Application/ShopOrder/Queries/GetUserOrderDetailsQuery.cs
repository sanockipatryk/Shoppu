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

namespace Shoppu.Application.ShopOrder.Queries
{
    public record GetUserOrderDetailsQuery(IPrincipal User, int OrderNumber) : IRequest<OrderDetailsViewModel>;
    public class GetUserOrderDetailsQueryHandler : IRequestHandler<GetUserOrderDetailsQuery, OrderDetailsViewModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetUserOrderDetailsQueryHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<OrderDetailsViewModel> Handle(GetUserOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.User.Identity.Name);

            var orderDetails = await _context.OrderDetails
                .Where(od => od.Order.UserId == user.Id && od.Order.Number == request.OrderNumber)
                .Select(od => new OrderDetails
                {
                    FirstName = od.FirstName,
                    LastName = od.LastName,
                    PostalCode = od.PostalCode,
                    City = od.City,
                    Street = od.Street,
                    Apartment = od.Apartment,
                    Phone = od.Phone,
                    Email = od.Email,
                    PaymentMethod = od.PaymentMethod,
                    TotalPrice = od.TotalPrice,
                    Order = new Order
                    {
                        Number = od.Order.Number,
                        UserId = od.Order.UserId,
                        Items = od.Order.Items.Select(odi => new OrderItem
                        {
                            Quantity = odi.Quantity,
                            ProductVariantSize = new ProductVariantSize
                            {
                                ProductVariant = new ProductVariant
                                {
                                    Name = odi.ProductVariantSize.ProductVariant.Name,
                                    Price = odi.ProductVariantSize.ProductVariant.Price,
                                    Slug = odi.ProductVariantSize.ProductVariant.Slug,
                                    Images = odi.ProductVariantSize.ProductVariant.Images.Select(img => new ProductVariantImage
                                    {
                                        ImageSource = img.ImageSource
                                    }).Take(1).ToList(),
                                    Product = new Product
                                    {
                                        Name = odi.ProductVariantSize.ProductVariant.Product.Name,
                                        Price = odi.ProductVariantSize.ProductVariant.Product.Price,
                                    }
                                },
                                Size = new Size
                                {
                                    Name = odi.ProductVariantSize.Size.Name,
                                }
                            }
                        }).ToList()
                    }
                }).FirstOrDefaultAsync();

            return new OrderDetailsViewModel
            {
                OrderDetails = orderDetails
            };

        }
    }
}