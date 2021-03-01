using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("Designation")]
    public  class Designation
    {
        public Designation()
        {
            EmployeeInfo = new HashSet<EmployeeInfo>();
        }

        public int DesignationId { get; set; }

        [StringLength(100)]
        public string DesignationName { get; set; }

        public bool IsActive { get; set; }

        public  ICollection<EmployeeInfo> EmployeeInfo { get; set; }
    }
}
