using Nowcfo.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface ISalesForecastRepository
    {
        Task<SalesForecastDto> GetByIdAsync(int id);
        Task<List<SalesForecastDto>> GetAllAsync();
        Task CreateAsync(SalesForecastDto model);
        Task Update(SalesForecastDto model);
        void Delete(SalesForecastDto model);

    }
}
