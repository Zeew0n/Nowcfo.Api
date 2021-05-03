using System;
using System.Collections.Generic;

namespace Nowcfo.Application.Dtos
{
    public class MarketMasterDto
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public DateTime PayPeriod { get; set; }
        public int AllocationTypeId { get; set; }
        public List<MarketAllocationDto> MarketAllocationDto { get; set; }


    }
}
