using System.Collections.Generic;

namespace Nowcfo.Application.Dtos
{
    public class MarketMasterDto
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string PayPeriod { get; set; }
        public int AllocationTypeId { get; set; }
        public string AllocationName { get; set; }
        public List<MarketAllocationDto> MarketAllocations { get; set; }


    }
}
