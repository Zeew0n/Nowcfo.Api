using Microsoft.AspNetCore.Identity;
using Nowcfo.Application.Dtos.Role;
using Nowcfo.Domain.Models.AppUserModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.Services.RoleService
{
    public interface IRoleService
    {
        Task CreateAsync(AppRole role);

        Task<IdentityResult> AddToRoleAsync(AppUser userIdentity, string role);

        Task<IdentityResult> RemoveFromRoleAsync(AppUser appUser);

        Task<string> GetRoleNameByIdAsync(Guid roleId);


        //
        Task<RoleDto> CreateAsync(RoleDto role);
        Task<bool> UpdateAsync(RoleDto role);
        Task<bool> DeleteAsync(Guid Id);
        Task<List<RoleDto>> GetAllAsync(Guid? id = null);
        Task<RoleDto> GetByIdAsync(Guid id);
    }
}