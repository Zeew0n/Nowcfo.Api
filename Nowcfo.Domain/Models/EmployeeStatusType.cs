using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{

    [Table("EmployeeStatusType")]
    public class EmployeeStatusType
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public EmployeeInfo Employee { get; set; }

    }
}
