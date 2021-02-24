using Nowcfo.Application.DTO;
using Nowcfo.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IEmployeeRepository
    {
        Task<EmployeeInfoDto> GetByIdAsync(int id);
        Task<List<EmployeeInfoDto>> GetAllAsync();
        Task CreateAsync(EmployeeInfo model);
        void Update(EmployeeInfo model);
        void Delete(EmployeeInfo model);
    }
}
