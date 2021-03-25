using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Nowcfo.API.Models;
using Nowcfo.Domain.Models.AppUserModels;
using Nowcfo.Infrastructure.Data;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.API.Helpers
{
    public static class DbInitializer
    {
        public static async Task<bool> InitializeUserRolesAsync(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            try
            {
                if (!context.Roles.Any() && !context.Users.Any())
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    string filePath = path.Replace("\\bin\\Debug\\net5.0", "") + "Helpers\\InitialUser.json";
                    using (StreamReader r = new StreamReader(filePath))
                    {
                        string json = r.ReadToEnd();
                        var items = JsonConvert.DeserializeObject<InitialUser>(json);
                        await CreateRolesAsync(roleManager, items);
                        await CreateUsersAsync(userManager, items);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                return false;
            }
        }
        private static async Task CreateRolesAsync(RoleManager<AppRole> roleManager, InitialUser items)
        {
            foreach (var roleName in items.RoleData)
            {
                var role = new AppRole()
                {
                    Name = roleName.Name,
                };
                await roleManager.CreateAsync(role);
            }
        }

        private static async Task CreateUsersAsync(UserManager<AppUser> userManager, InitialUser items)
        {
            foreach (var item in items.UserData)
            {
                var userName = new AppUser
                {
                    UserName = item.UserName,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    IsAdmin = item.IsAdmin,
                    EmailConfirmed = item.EmailConfirmed
                };
                string userPassword = item.Password;
                var createUser = await userManager.CreateAsync(userName, userPassword);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(userName, item.Role);
                }
            }
        }
    }
}