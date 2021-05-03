using System;

namespace Nowcfo.Application.Dtos
{
    public class MarketAllocationDto
    {
        public int Id { get; set; }
        public int MarketId { get; set; }
        public int MasterId { get; set; }
        public Decimal Revenue { get; set; }
        public Decimal COGS { get; set; }
        public int CogsTypeId { get; set; }
        public Decimal SE { get; set; }
        public Decimal EE { get; set; }
        public Decimal GA { get; set; }
        public Decimal Other { get; set; }
        public int OtherTypeId { get; set; }

    }
}
