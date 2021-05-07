using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("JobRoleHistorical")]
    public class JobRoleHistorical
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int RoleId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
