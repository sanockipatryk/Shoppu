using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Shoppu.Application.ShoppingCart.Queries
{
    public record GetCartForUserQuery() : IRequest<int>;

    // public class GetCartForUserQueryHandler : IRequestHandler<GetCartForUserQuery, int>
    // {

    // }
}