using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("EmployeeStatusHistorical")]
    public class EmployeeStatusHistorical
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int StatusId { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
