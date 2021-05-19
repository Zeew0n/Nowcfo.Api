using System.Collections.Generic;

namespace Nowcfo.Application.Dtos
{
    public class OrganizationAllocationDto
    {
        public string OrganizationName { get; set; }
        public List<MarketDto> Markets { get; set; }
    }

    public class MarketDto
    {
        public int MarketId { get; set; }
        public string MarketName { get; set; }
    }
}
