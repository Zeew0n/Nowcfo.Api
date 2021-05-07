using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("PayTypeHistorical")]

    public class PayTypeHistorical
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? PayType { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
