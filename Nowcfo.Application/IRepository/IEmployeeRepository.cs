using Nowcfo.Application.Dtos;
using Nowcfo.Application.Helper.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IEmployeeRepository
    {
        Task<EmployeeInfoDto> GetByIdAsync(int id);
        Task<List<EmployeeInfoDto>> GetAllAsync();
        Task<PagedList<EmployeeInfoDto>> GetPagedListAsync(Param param);

        Task<List<EmployeeInfoDto>> GetAllSuperVisors();
        Task CreateAsync(EmployeeInfoDto model);
        void Update(EmployeeInfoDto model);
        void Delete(EmployeeInfoDto model);
        Task<List<EmployeeInfoDto>> GetEmployeesAutocompleteAsync(string searchText);

        //
        Task<List<SyncfusionListDto>> GetEmployeePermissionHierarchy(int employeeId);
        Task<List<SyncfusionListDto>> GetSyncFusionOrganizations();
        Task<List<UserPermissionDto>> GetCheckedPermissions(int employeeId);
    }
}