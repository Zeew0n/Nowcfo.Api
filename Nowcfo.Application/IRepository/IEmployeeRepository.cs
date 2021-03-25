using Nowcfo.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IEmployeeRepository
    {
        Task<EmployeeInfoListDto> GetByIdAsync(int id);
        Task<List<EmployeeInfoListDto>> GetAllAsync();
        Task<List<EmployeeInfoDto>> GetAllSuperVisors();
        Task CreateAsync(EmployeeUpdateDto model);
        void Update(EmployeeUpdateDto model);
        void Delete(EmployeeInfoListDto model);
        Task<List<KendoDto>> GetKendoTreeHierarchy();
        Task<List<EmployeeOrganizationPermissionNavDto>> GetEmployeePermissionHierarchy(int employeeId);
    }
}