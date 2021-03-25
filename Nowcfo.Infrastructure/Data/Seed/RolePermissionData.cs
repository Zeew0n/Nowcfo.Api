using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Dtos.Role;
using Nowcfo.Application.Exceptions;
using Nowcfo.Application.Helper;
using Nowcfo.Domain.Models;
using Nowcfo.Domain.Models.AppUserModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Nowcfo.Infrastructure.Data.Seed
{
    public class RolePermissionData
    {
        public static async  Task<bool> SeedPermissionsForRole(ApplicationDbContext context)
        {
            try
            {
                string[] roles =
                {
                    DesignationAndRoleConstants.SuperAdmin, DesignationAndRoleConstants.Admin,
                    DesignationAndRoleConstants.User
                };

                var result = await context.Roles.Where(q => roles.Contains(q.NormalizedName))
                    .Select(q => new RoleDto
                    {
                        RoleId = q.Id,
                        RoleName = q.NormalizedName
                    }).ToListAsync();
                var permissions = await context.Permissions.ToListAsync();
                var rolePermissions = await context.RolePermissions.ToListAsync();

                if (rolePermissions.Count > 0)
                {
                    context.RolePermissions.RemoveRange(rolePermissions);
                    await context.SaveChangesAsync();
                }

                if (result.Count == 0)
                {
                    Log.Error("No records in role table.");
                    throw new ApiException("No records in role table.");
                }

                if (permissions.Count == 0)
                {
                    Log.Error("No records in permission table.");
                    throw new ApiException("No records in permission table.");
                }

                //await SeedSuperAdminPermissions(context, result, permissions);
                await SeedAdminPermissions(context, result, permissions);
                await SeedUserPermissions(context, result, permissions);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failed to seed RolePermissionMappings");
                return false;
            }
        }



        private static async Task SeedSuperAdminPermissions (ApplicationDbContext context, List<RoleDto> roles,
            List<Permission> permissions)
        {
            Guid roleId = roles.Where(q => q.RoleName == DesignationAndRoleConstants.SuperAdmin)
                .Select(q => q.RoleId).FirstOrDefault();
            var rolePermission = new List<RolePermission>()
            {
                //Role
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.ViewUser(context), permissions)
                },
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.ViewRole(context), permissions)
                },
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.UpdateRole(context), permissions)
                },
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.DeleteRole(context), permissions)
                },

                //User
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.AddUser(context), permissions)
                },
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.ViewUser(context), permissions)
                },
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.UpdateUser(context), permissions)
                },
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.DeleteUser(context), permissions)
                }
            };
            await context.RolePermissions.AddRangeAsync(rolePermission);
        }

        private static async Task SeedAdminPermissions(ApplicationDbContext context, List<RoleDto> roles,
            List<Permission> permissions)
        {
            Guid roleId = roles.Where(q => q.RoleName == DesignationAndRoleConstants.Admin)
                .Select(q => q.RoleId).FirstOrDefault();
            var rolePermission = new List<RolePermission>()
            {
                //Role
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.AddRole(context), permissions)
                },
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.ViewRole(context), permissions)
                },

                //User
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.AddUser(context), permissions)
                },
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.ViewUser(context), permissions)
                },

                
                //employee
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.AddEmployee(context), permissions)
                },
                new RolePermission
                {
                     RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.ViewEmployee(context), permissions)
                }



            };
            await context.RolePermissions.AddRangeAsync(rolePermission);
        }

        private static async Task SeedUserPermissions(ApplicationDbContext context, List<RoleDto> roles,
            List<Permission> permissions)
        {
            Guid roleId = roles.Where(q => q.RoleName == DesignationAndRoleConstants.User).Select(q => q.RoleId)
                .FirstOrDefault();
            var rolePermission = new List<RolePermission>()
            {
                //Role
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.ViewRole(context), permissions)
                },
                //User
                new RolePermission
                {
                    RoleId = roleId, PermissionId = GetPermissionIdByName(PerOption.ViewUser(context), permissions)
                }
          
            };
            await context.RolePermissions.AddRangeAsync(rolePermission);
        }

        private static Guid GetPermissionIdByName(string name, List<Permission> permissions)
        {
            return permissions.Where(q => q.Slug == name).Select(q => q.Id).FirstOrDefault();
        }
    }
}