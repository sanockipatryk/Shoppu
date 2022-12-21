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

namespace Shoppu.Application.ShoppingCart.Queries
{
    public record GetCartForUserQuery(IPrincipal User) : IRequest<int>;

    public class GetCartForUserQueryHandler : IRequestHandler<GetCartForUserQuery, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GetCartForUserQueryHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Handle(GetCartForUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.User.Identity.Name);

            var userCart = await _context.Carts
                .Where(c => c.UserId == user.Id)
                .Select(c => new Cart
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Items = c.Items.Select(ci => new CartItem
                    {
                        Id = ci.Id,
                        ProductVariantSizeId = ci.ProductVariantSizeId,
                        Quantity = ci.Quantity,
                    }).ToList(),
                })
                .FirstOrDefaultAsync();

            if (userCart == null)
            {
                userCart = new Cart
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                };

                await _context.Carts.AddAsync(userCart);
                await _context.SaveChangesAsync();
                return 0;
            }

            return userCart.ItemsCount;
        }
    }
}