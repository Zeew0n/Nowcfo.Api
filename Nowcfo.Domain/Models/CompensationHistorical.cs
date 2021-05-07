using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("CompensationHistorical")]
    public class CompensationHistorical
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Pay { get; set; }
        public string OverTimeRate { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
