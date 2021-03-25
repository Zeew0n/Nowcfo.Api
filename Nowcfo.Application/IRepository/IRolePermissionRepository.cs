using Nowcfo.Application.Dtos;
using Nowcfo.Domain.Models.AppUserModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IRolePermissionRepository
    {
        Task<RolePermissionDto> GetByIdAsync(Guid id);
        Task<List<RolePermissionDto>> GetAllAsync();
        Task CreateAsync(RolePermission model);
        void Update(RolePermission model);
        void Delete(RolePermission model);
    }
}