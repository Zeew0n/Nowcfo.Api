using Nowcfo.Application.DTO;
using Nowcfo.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IOrganizationRepository
    {
        Task<OrganizationDto> GetByIdAsync(int id);
        Task<List<OrganizationDto>> GetAllAsync();
        Task CreateAsync(Organization model);
        void Update(Organization model);
        void Delete(Organization model);
    }
}
