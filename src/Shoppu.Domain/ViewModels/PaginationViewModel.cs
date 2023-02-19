namespace Shoppu.Domain.ViewModels
{
    public class PaginationViewModel
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalItemsCount { get; set; }
        public int ItemsPerPage { get; set; }
    }
}