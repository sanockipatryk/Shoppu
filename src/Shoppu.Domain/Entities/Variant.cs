using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.Entities
{
    public class Variant
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
