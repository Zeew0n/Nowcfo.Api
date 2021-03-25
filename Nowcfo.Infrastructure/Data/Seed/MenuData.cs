using Nowcfo.Domain.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.Infrastructure.Data.Seed
{
    public static class MenuData
    {
        public static async Task<bool> SeedMenus(ApplicationDbContext context)
        {
            var menus = GetMenuSeed();
            var existingMenus = context.Menus.ToList();
            var existingIds = existingMenus.Select(s => s.Id).ToList();

            if (existingMenus.Count == 0)
            {
                context.Menus.AddRange(menus);
                await context.SaveChangesAsync();
                return true;
            }

            if (menus.GroupBy(q => q.Id).Any(drg => drg.Count() > 1))
            {
                Log.Error("Error: Duplicate Menu added.");
                return false;
            }

            var newMenus = menus.Where(q => !existingIds.Contains(q.Id)).ToArray();
            if (newMenus.Any())
            {
                context.Menus.AddRange(newMenus);
                await context.SaveChangesAsync();
                return true;
            }

            var oldMenus = existingMenus.Where(q => !menus.Select(r => r.Id).Contains(q.Id)).ToArray();
            if (oldMenus.Any())
            {
                context.Menus.RemoveRange(oldMenus);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }


        private static List<Menu> GetMenuSeed()
        {

            return new List<Menu>
            {
                new Menu(Guid.Parse("B9E3E133-30FE-4F6B-ABC3-8F7F1DBEE42A"),"Role"),
                new Menu(Guid.Parse("28110F91-81EB-4DED-841E-D5CF1EC0A8D2"),"User"), 
                new Menu(Guid.Parse("E5530EA2-26F9-4139-8489-83B0A08D22A9"),"Employee"), 
                new Menu(Guid.Parse("80365B91-1BBB-4F34-B6C9-4A35FB09F121"),"Organization"), 
                new Menu(Guid.Parse("F2CE83CC-4D9E-44FD-A4AA-2AF8F88F4CC8"), "Designation")

            };
        }
    }
}
