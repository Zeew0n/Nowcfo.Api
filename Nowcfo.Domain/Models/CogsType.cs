using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("CogsType")]
    public class CogsType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
