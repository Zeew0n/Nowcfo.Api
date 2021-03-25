using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nowcfo.Domain.Models;
using Nowcfo.Domain.Models.AppUserModels;

namespace Nowcfo.Infrastructure.Data
{
    public  class ConfigureRelation
    {
        public static void ConfigurePermission(EntityTypeBuilder<Permission> builder)
        {
            builder.HasOne(q => q.Menu)
                .WithMany(q=>q.Permissions)
                .HasForeignKey(q=>q.MenuId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public static void ConfigureRolePermission(EntityTypeBuilder<RolePermission> builder)
        {

            builder.HasKey(q => new { q.RoleId, q.PermissionId });
            
            builder.HasOne(q => q.Role)
                .WithMany(q => q.RolePermissions)
                .HasForeignKey(q => q.RoleId);
            
            builder.HasOne(q => q.Permission)
                .WithMany(q => q.RolePermissions)
                .HasForeignKey(q => q.PermissionId);
        }

        public static void ConfigureEmpOrgPermission(EntityTypeBuilder<EmployeeOrgPermission> builder)
        {
            builder.HasKey(q => q.EmployeeOrganizationPermissionId);
            builder.Property(p => p.Employee_Id).IsRequired(false);
            builder.Property(p => p.Organization_Id).IsRequired(false);
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
