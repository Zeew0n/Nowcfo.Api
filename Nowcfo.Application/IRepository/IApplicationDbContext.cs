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
        public DbSet<EmployeePermission> EmployeePermissions { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<EmployeeStatusType> EmployeeStatusTypes { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<DynamicFilterField> DynamicFilterFields { get; set; }
        public DbSet<MarketMaster> MarketMasters { get; set; }
        public DbSet<MarketAllocation> MarketAllocations { get; set; }
        public DbSet<AllocationType> AllocationTypes { get; set; }
        public DbSet<OtherType> OtherTypes { get; set; }
        public DbSet<CogsType> CogsTypes { get; set; }
    }

}
