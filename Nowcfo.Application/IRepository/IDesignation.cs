using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nowcfo.Application.DTO;
using Nowcfo.Domain.Models;

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
