using System.ComponentModel.DataAnnotations;

namespace Shoppu.Domain.Entities
{
    public class Size
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
