using Nowcfo.Application.Dtos;
using Nowcfo.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IDesignation
    {
        Task<DesignationDto> GetByIdAsync(int id);
        Task<List<DesignationDto>> GetAllAsync();
        Task CreateAsync(Designation model);
        void Update(Designation model);
        void Delete(Designation model);
    }
}
