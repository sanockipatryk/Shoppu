using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.Enums
{
    public enum PaymentMethod
    {
        [Display(Name = "Credit card")]
        CreditCard = 1,
        [Display(Name = "Paypal")]
        Paypal = 2,
        [Display(Name = "Online transfer")]
        OnlineTransfer = 3

    }
}
