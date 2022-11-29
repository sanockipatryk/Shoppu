using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class Cart
    {
        public string Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<CartItem> Items { get; set; }

        public bool IsEmpty => !Items.Any();
        public int ItemsCount => Items.Sum(i => i.Quantity);
    }
}
