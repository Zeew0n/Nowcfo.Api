using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("MarketMaster")]
    public class MarketMaster: BaseEntity, ISoftDeletableEntity
    {
        public MarketMaster()
        {
            MarketAllocations = new HashSet<MarketAllocation>();
        }
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public DateTime PayPeriod { get; set; }
        public int AllocationTypeId { get; set; }
        public ICollection<MarketAllocation> MarketAllocations { get; set; }

    }
}
