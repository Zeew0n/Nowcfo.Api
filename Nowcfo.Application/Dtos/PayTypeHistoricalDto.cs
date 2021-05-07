using System;

namespace Nowcfo.Application.Dtos
{
    public class PayTypeHistoricalDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? PayType { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
