using Microsoft.EntityFrameworkCore;
using Shoppu.Domain.ViewModels;

namespace Shoppu.Application.Helpers
{
    public static class PaginationHelpers
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, ref PaginationViewModel pagination) where T : class
        {
            pagination.TotalItemsCount = query.Count();
            pagination.TotalPages = PaginationHelpers.GetTotalPages(pagination);

            if (pagination.TotalItemsCount > 0)
            {
                pagination.Page = GetMinOrMaxPageIfOverBounds(pagination);
                return query
                    .Skip((pagination.Page - 1) * pagination.ItemsPerPage)
                    .Take(pagination.ItemsPerPage);
            }
            return query;
        }

        public static int GetTotalPages(PaginationViewModel pagination)
        {
            return (int)Math.Ceiling((double)pagination.TotalItemsCount / pagination.ItemsPerPage);
        }

        public static int GetMinOrMaxPageIfOverBounds(PaginationViewModel pagination)
        {
            if (pagination.Page > pagination.TotalPages)
            {
                return pagination.TotalPages;
            }
            if (pagination.Page < 1)
            {
                return 1;
            }
            else
            {
                return pagination.Page;
            }
        }
    }
}