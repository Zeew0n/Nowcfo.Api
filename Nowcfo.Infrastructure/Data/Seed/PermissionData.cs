using Nowcfo.Application.Helper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Permission = Nowcfo.Domain.Models.Permission;

namespace Nowcfo.Infrastructure.Data.Seed
{
    public class PermissionData
    {
        public static async Task<bool> SeedPermissions(ApplicationDbContext context)
        {
            var permissions = GetPermissionSeed(context);
            var existingPermissions = context.Permissions.ToList();
            var existingIds = existingPermissions.Select(s => s.Id).ToList();

            if (existingPermissions.Count == 0)
            {
                context.Permissions.AddRange(permissions);
                await context.SaveChangesAsync();
                return true;
            }

            if (permissions.GroupBy(q => q.Id).Any(drg => drg.Count() > 1))
            {
                Log.Error("Error: Duplicate permission added.");
                return false;
            }

            var newPermissions = permissions.Where(q => !existingIds.Contains(q.Id)).ToArray();
            if (newPermissions.Any())
            {
                context.Permissions.AddRange(newPermissions);
                await context.SaveChangesAsync();
                return true;
            }

            var oldPermissions = existingPermissions.Where(q => !permissions.Select(r => r.Id).Contains(q.Id)).ToArray();
            if (oldPermissions.Any())
            {
                context.Permissions.RemoveRange(oldPermissions);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }


        private static List<Permission> GetPermissionSeed(ApplicationDbContext context)
        {
            var menus = context.Menus.ToList();
            var permissions = new List<Permission>();
            foreach (var menu in menus)
            {
                var viewPermission = 
                    new Permission(Guid.NewGuid(),  
                    $"{menu.MenuName}.read".ToUpper(), $"{menu.MenuName}.read".ToLower(), Group.User,menu.Id);
                var addPermission =
                    new Permission(Guid.NewGuid(),
                        $"{menu.MenuName}.create".ToUpper(), $"{menu.MenuName}.create".ToLower(), Group.User, menu.Id);
                var updatePermission =
                    new Permission(Guid.NewGuid(),
                        $"{menu.MenuName}.update".ToUpper(), $"{menu.MenuName}.update".ToLower(), Group.User, menu.Id);
                var deletePermission =
                    new Permission(Guid.NewGuid(),
                        $"{menu.MenuName}.delete".ToUpper(), $"{menu.MenuName}.delete".ToLower(), Group.User, menu.Id);
                
                permissions.Add(viewPermission);
                permissions.Add(addPermission);
                permissions.Add(updatePermission);
                permissions.Add(deletePermission);

            }

            return permissions;
        }
    }
}