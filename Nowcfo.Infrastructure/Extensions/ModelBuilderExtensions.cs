using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Domain.Models.AppUserModels;
using System;

namespace Nowcfo.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SeedAdminUser(this ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<AppRole>().HasData(
                new AppRole
                {
                    Id = Guid.Parse("CC086577-D584-404A-BB5C-B44166199B01"),
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin".ToUpper()

                },
                new AppRole
                {
                    Id = Guid.Parse("92DEA008-9D4B-4C59-904D-7F5A700E67AE"),
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                },
                new AppRole
                {
                    Id = Guid.Parse("31BFDEF9-6776-4156-B727-5E8FF2A12573"),
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                }
            );



            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<AppUser>();


            //Seeding the User to AspNetUsers table
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = Guid.Parse("B3BB50EF-D624-41DE-A93B-2031D0FD392E"), // primary key
                    UserName = "superadmin",
                    NormalizedUserName = "superadmin".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "Devfinity#$123"),
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    CreatedDate = DateTime.Now,
                    CreatedBy = Guid.NewGuid(),
                    Email = "merolook@outlook.com",
                    EmailConfirmed = true
                }
            );


            //Seeding the relation between our user and role to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = Guid.Parse("CC086577-D584-404A-BB5C-B44166199B01"),
                    UserId = Guid.Parse("B3BB50EF-D624-41DE-A93B-2031D0FD392E")
                }
            );
        }



    }
}
