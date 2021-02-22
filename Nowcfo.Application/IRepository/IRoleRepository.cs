using Nowcfo.Application.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IRoleRepository
    {
        Task<RoleDto> CreateAsync(RoleDto role);
        Task<bool> UpdateAsync(RoleDto role);
        Task<bool> DeleteAsync(Guid Id);
        Task<List<RoleDto>> GetAllAsync(Guid? id = null);
        Task<RoleDto> GetByIdAsync(Guid id);
    }
}