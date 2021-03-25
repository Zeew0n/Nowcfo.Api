using Nowcfo.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    interface IEmployee
    {
        Task<EmployeeInfoDto> GetByIdAsync(int id);
        Task<List<EmployeeInfoDto>> GetAllAsync();
        Task CreateAsync(EmployeeInfoDto model);
        void Update(EmployeeInfoDto model);
        void Delete(EmployeeInfoDto model);
    }
}
