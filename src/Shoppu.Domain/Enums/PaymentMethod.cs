using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.Enums
{
    public enum PaymentMethod
    {
        [Display(Name = "BLIK")]
        Blik = 1,
        [Display(Name = "Online transfer")]
        OnlineTransfer = 2,
        [Display(Name = "On delivery")]
        OnDelivery = 3

    }
}
