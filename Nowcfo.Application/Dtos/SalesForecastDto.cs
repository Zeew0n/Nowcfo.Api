using System;

namespace Nowcfo.Application.Dtos
{
    public class SalesForecastDto
    {
        public int Id { get; set; }
        public string PayPeriod { get; set; }
        public Decimal BillRate { get; set; }
        public Decimal BillHours { get; set; }
        public Decimal Placements { get; set; }
        public Decimal BuyOuts { get; set; }
        public Decimal EstimatedRevenue { get; set; }
        public Decimal Cogs { get; set; }
        public Decimal CogsQkly { get; set; }
        public Decimal ClosedPayPeriods { get; set; }
        public Decimal OtherPercent { get; set; }
    }
}
