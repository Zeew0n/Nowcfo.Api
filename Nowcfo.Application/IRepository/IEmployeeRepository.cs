using Nowcfo.Application.Dtos;
using Nowcfo.Application.Helper.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IEmployeeRepository
    {
        Task<EmployeeInfoListDto> GetByIdAsync(int id);
        Task<PagedList<EmployeeInfoListDto>> GetAllAsync(Param param);
        Task<List<EmployeeInfoDto>> GetAllSuperVisors();
        Task CreateAsync(EmployeeUpdateDto model);
        void Update(EmployeeUpdateDto model);
        void Delete(EmployeeInfoListDto model);
        Task<List<KendoDto>> GetKendoTreeHierarchy();
        Task<List<EmployeeOrganizationPermissionNavDto>> GetEmployeePermissionHierarchy(int employeeId);
    }
}