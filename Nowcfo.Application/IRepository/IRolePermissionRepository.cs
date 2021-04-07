using Nowcfo.Application.Dtos;
using System;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IRolePermissionRepository
    {
        Task<RolePermissionDto> GetByIdAsync(Guid id);
    }
}