using Nowcfo.Application.Dtos;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IMarketAllocationRepository
    {
        //List Market Allocation List
        //Task<MarketMasterDto> GetByOrganizationIdAsync(int id);

        //To Load Bottom Level Children based on Create/Update
        //Task<MarketAllocationDto> GetByMasterId(int id, string type);

        Task CreateAsync(MarketMasterDto model);
        void Update(MarketMasterDto model);
        void Delete(MarketMasterDto model);

    }
}
