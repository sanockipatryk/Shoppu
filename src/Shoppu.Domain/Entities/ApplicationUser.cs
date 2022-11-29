using Microsoft.AspNetCore.Identity;

namespace Shoppu.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public List<Order> Orders { get; set; }
    }
}
