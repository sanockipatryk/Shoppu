using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Shoppu.Application.ShoppingCart.Commands
{
    public record AddProductVariantToCartCommand() : IRequest<int>;

}