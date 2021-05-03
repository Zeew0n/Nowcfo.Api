using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("AllocationType")]
    public class AllocationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
