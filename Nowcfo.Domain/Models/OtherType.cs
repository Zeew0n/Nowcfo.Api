using System.ComponentModel.DataAnnotations.Schema;
namespace Nowcfo.Domain.Models
{
    [Table("OtherType")]
    public class OtherType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
