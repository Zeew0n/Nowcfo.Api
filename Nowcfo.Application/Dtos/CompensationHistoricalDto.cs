using System;

namespace Nowcfo.Application.Dtos
{
    public class CompensationHistoricalDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Pay { get; set; }
        public string OverTimeRate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
