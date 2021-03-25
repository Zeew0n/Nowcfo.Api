using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Nowcfo.Infrastructure.Extensions
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData
        (this ModelBuilder modelBuilder,UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers
            (UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync
                ("super").Result == null)
            {
                IdentityUser user = new IdentityUser();
                user.UserName = "super";
                user.Email = "merolook@outlook.com";

                IdentityResult result = userManager.CreateAsync
                    (user, "Devfinity#$123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                        "SuperAdmin").Wait();
                }
            }
        }

        public static void SeedRoles
            (RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync
                ("SuperAdmin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "SuperAdmin";
                role.NormalizedName = role.Name.ToUpper();
                IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync
                ("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                role.NormalizedName = role.Name.ToUpper();
                IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync
                ("User").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User";
                role.NormalizedName = role.Name.ToUpper();
                IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
            }
        }


    }
}
