using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("MarketAllocation")]

    public class MarketAllocation:BaseEntity, ISoftDeletableEntity
    {

        public int Id { get; set; }
        public int MarketId { get; set; }
        public int MasterId { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public Decimal Revenue { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public Decimal COGS { get; set; }
        public int CogsTypeId {get; set;}
        [Column(TypeName = "decimal(18,4)")]
        public Decimal SE { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public Decimal EE { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public Decimal GA { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public Decimal Other { get; set; }
        public int OtherTypeId { get; set; }
        public MarketMaster MarketMaster { get; set; }


    }
}
