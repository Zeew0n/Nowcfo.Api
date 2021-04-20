using System.Collections.Generic;
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
        public ICollection<EmployeeInfo> Employees { get; set; }

    }
}
