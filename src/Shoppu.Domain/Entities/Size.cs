using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shoppu.Domain.Entities
{
    public class Size
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey(nameof(SizeType))]
        public int SizeTypeId { get; set; }
        public SizeType SizeType { get; set; }
    }
}
