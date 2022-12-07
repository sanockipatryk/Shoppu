using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class Variant
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string HEXColor { get; set; }
    }
}
