using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nowcfo.Domain.Models;
using Nowcfo.Domain.Models.AppUserModels;
using Nowcfo.Domain.Models.User;

namespace Nowcfo.Infrastructure.Data
{
    public  class ConfigureRelation
    {
        public static void ConfigureRolePermissionMapping(EntityTypeBuilder<RolePermissionMapping> builder)
        {

            builder.HasKey(q => new { q.RoleId, q.PermissionId });
            
            builder.HasOne(q => q.Role)
                .WithMany(q => q.RolePermissions)
                .HasForeignKey(q => q.RoleId);
            
            builder.HasOne(q => q.Permission)
                .WithMany(q => q.RolePermissions)
                .HasForeignKey(q => q.PermissionId);
        }

        public static void ConfigureUserSignup(EntityTypeBuilder<UserSignUp> builder)
        {
            builder.HasKey(q => q.UserId);
        }



        public static void ConfigureAppUser(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(p => p.Address).IsRequired(false);
            builder.Property(p => p.City).IsRequired(false);
            builder.Property(p => p.State).IsRequired(false);
            builder.Property(p => p.ZipCode).IsRequired(false);
        }


        public static void ConfigureEmployeeInfo(EntityTypeBuilder<EmployeeInfo> builder)
        {
            builder
                .HasMany(e => e.EmployeeInfos)
                .WithOne(e => e.Employee)
                .HasForeignKey(e => e.SupervisorId)
                
                .OnDelete(DeleteBehavior.Restrict);

        }

        public static void ConfigureMenu(EntityTypeBuilder<Menu> builder)
        {

            builder
                .HasMany(e => e.Menus)
                .WithOne(e => e.MenuOne)
                .HasForeignKey(e => e.UnderMenuId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(e => e.MenuPermissions)
                .WithOne(e => e.Menu)
                .HasForeignKey(e=>e.MenuId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public static void ConfigureOrganization(EntityTypeBuilder<Organization> builder)
        {
            builder
                .HasMany(e => e.Organizations)
                .WithOne(e => e.OneOrganization)
                .HasForeignKey(e => e.ParentOrganizationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.HasParent)
                .HasDefaultValue(false);
        }
    }
}
