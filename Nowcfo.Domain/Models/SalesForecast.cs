using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nowcfo.Domain.Models
{
    [Table("SalesForecast")]
    public class SalesForecast : BaseEntity, ISoftDeletableEntity
    {

        public int Id { get; set; }
        public DateTime PayPeriod { get; set; }
        [Column(TypeName="decimal(18,4)")]
        public Decimal BillRate { get; set; }
        [Column(TypeName="decimal(18,4)")]
        public Decimal BillHours { get; set; }
        [Column(TypeName="decimal(18,4)")]
        public Decimal Placements { get; set; }
        [Column(TypeName="decimal(18,4)")]
        public Decimal BuyOuts { get; set; }
        [Column(TypeName="decimal(18,4)")]
        public Decimal EstimatedRevenue { get; set; }
        [Column(TypeName="decimal(18,4)")]
        public Decimal Cogs { get; set; }
        [Column(TypeName="decimal(18,4)")]
        public Decimal CogsQkly { get; set; }
        [Column(TypeName="decimal(18,4)")]
        public Decimal ClosedPayPeriods { get; set; }
        [Column(TypeName="decimal(18,4)")]
        public Decimal OtherPercent { get; set; }

    }
}
