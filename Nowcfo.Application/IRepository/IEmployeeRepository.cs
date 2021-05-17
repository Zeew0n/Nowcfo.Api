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
        Task<List<EmployeeStatusTypeDto>> GetAllEmployeeStatus();
        Task<List<EmployeeTypeDto>> GetAllEmployeeTypes();
        Task<List<EmployeeInfoDto>> GetAllSuperVisors();
        Task CreateAsync(EmployeeInfoDto model);
        Task  Update(EmployeeInfoDto model);
        void Delete(EmployeeInfoDto model);
        Task<List<EmployeeInfoDto>> GetEmployeesAutocompleteAsync(string searchText);
        bool CheckIfEmailExists(string email);


    }
}