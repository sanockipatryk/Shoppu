using MediatR;
using Shoppu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoppu.Application.Queries.Products
{
    public record GetProductListQuery() : IRequest<List<Product>>;
}
