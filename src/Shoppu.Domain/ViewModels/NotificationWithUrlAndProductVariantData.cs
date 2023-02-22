namespace Shoppu.Domain.ViewModels
{
    public class NotificationWithUrlAndProductVariantData : NotificationWithUrlData
    {
        public int ProductVariantId { get; set; }
        public string ProductVariantCode { get; set; }
    }
}