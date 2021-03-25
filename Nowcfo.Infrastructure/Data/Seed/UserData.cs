using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Exceptions;
using Nowcfo.Application.Helper;
using Nowcfo.Domain.Models.AppUserModels;
using Serilog;

namespace Nowcfo.Infrastructure.Data.Seed
{
    public class UserData
    {
        public static async Task<bool> SeedDefaultUserAsync(ApplicationDbContext context)
        {
            var existingUsers = await context.Users.ToListAsync();
            if (existingUsers.Count == 0)
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
                return true;
            }

            Log.Error("Initial seed already completed.");
            throw new ApiException("Initial seed already completed.");
        }
    }
}
