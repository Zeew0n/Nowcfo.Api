using Nowcfo.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IMarketAllocationRepository
    {
        //List Market Allocation List

        //To Load Bottom Level Children based on Create/Update for data change, what to do in case of change of data
        Task<List<MarketAllocationDto>> GetAllAllocationsById(int masterId, string payPeriod, int id, int allocationTypeId);
        Task<List<MarketAllocationDto>> GetAllMarketsByOrgId(int orgId);
        Task<MarketMasterDto> GetByIdAsync(int id);
        Task CreateAsync(MarketMasterDto model);
        Task Update(MarketMasterDto model);
        void Delete(MarketMasterDto model);
        Task<List<OrganizationDto>> GetAllOrganizations();
        Task<List<MarketMasterDto>> GetAllMarketList(int id);
        Task<List<AllocationTypeDto>> GetAllocationTypes();

        Task<List<CogsTypeDto>> GetCogsTypes();

        Task<List<OtherTypeDto>> GetOtherTypes();




    }
}
