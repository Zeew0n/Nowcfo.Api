using Nowcfo.Application.Dtos;
using Nowcfo.Application.Helper.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface ISalesForecastRepository
    {
        Task<PagedList<SalesForecastDto>> GetPagedListAsync(Param param);
        Task<SalesForecastDto> GetByIdAsync(int id);
        Task<List<SalesForecastDto>> GetAllAsync();
        Task CreateAsync(SalesForecastDto model);
        Task Update(SalesForecastDto model);
        void Delete(SalesForecastDto model);
        bool CheckIfPayPeriodExists(string payPeriod);

    }
}
