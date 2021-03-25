using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Domain.Models;
using Nowcfo.Domain.Models.AppUserModels;
using System;

namespace Nowcfo.Application.IRepository
{
    public interface IApplicationDbContext:IDbContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<IdentityUserRole<Guid>> UserRoles { get; set; }
        public DbSet<IdentityUserClaim<Guid>> UserClaims { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<EmployeeInfo> EmployeeInfos { get; set; }
        public DbSet<EmployeeOrgPermission> EmployeeOrgPermissions { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Menu> Menus { get; set; }
    }
}
