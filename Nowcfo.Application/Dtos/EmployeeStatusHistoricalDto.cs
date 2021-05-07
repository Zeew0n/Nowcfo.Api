using System;

namespace Nowcfo.Application.Dtos
{
    public class EmployeeStatusHistoricalDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int StatusId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
