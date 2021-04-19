using Nowcfo.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IEmployeePermissionRepository
    {
        Task<EmployeePermissionDto> GetByIdAsync(int id);
        Task<List<EmployeePermissionDto>> GetAllAsync();
        Task CreateAsync(EmployeePermissionDto model);
        void Update(EmployeePermissionDto model);

        Task<List<OrganizationDto>> GetLevelOrganizations(int organizationId);





    }
}
