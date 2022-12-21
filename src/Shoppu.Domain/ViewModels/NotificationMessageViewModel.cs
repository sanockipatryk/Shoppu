using Shoppu.Domain.Enums;

namespace Shoppu.Domain.ViewModels
{
    public class NotificationMessageViewModel
    {
        public StatusType StatusType { get; set; }
        public string? Message { get; set; }
    }
}