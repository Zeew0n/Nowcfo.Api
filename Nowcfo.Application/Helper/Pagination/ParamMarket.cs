using System;

namespace Nowcfo.Application.Helper.Pagination
{
    public class ParamMarket
    {
        private const int MaxPageSize = 1000000;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public Guid Id { get; set; }
        public int SearchOrg { get; set; }
    }
}
