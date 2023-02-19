using Shoppu.Domain.Enums;

namespace Shoppu.Domain.ViewModels
{
    public class UserOrderViewModel
    {
        public int OrderNumber { get; set; }
        public DateTime DateOrdered { get; set; }
        public int NumberOfItems { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}