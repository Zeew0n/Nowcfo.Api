
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Helper;
using Nowcfo.Domain.Models.AppUserModels;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.Infrastructure.Data.Seed
{
    public class RoleData
    {
        public static async Task SeedDefaultRolesAsync(ApplicationDbContext context)
        {
            try
            {
                var existingRoles = await context.Roles.ToListAsync();
                if (existingRoles.Count == 0)
                {
                    var roles = DesignationAndRoleConstants.GetDefaultRoles();
                    var entities = roles.Select(q => new AppRole
                    {
                        Id = q.Id,
                        Name = q.Name,
                        NormalizedName = q.NormalizedName
                    });
                    context.Roles.AddRange(entities);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Log.Error(e,"Failed to seed Roles");
            }
           
        }
    }
}