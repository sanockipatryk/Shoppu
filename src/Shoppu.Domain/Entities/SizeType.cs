using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class SizeType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Product> Products { get; set; }
    }
}
