using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoppu.Domain.ViewModels
{
    public class UserOrderListViewModel
    {
        public List<UserOrderViewModel>? OrderList { get; set; }
        public PaginationViewModel Pagination { get; set; }
    }
}