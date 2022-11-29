using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class Order
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public DateTime DateOrdered { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<OrderItem> Items { get; set; }
    }
}
