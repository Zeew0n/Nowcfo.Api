using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.IRepository;
using Nowcfo.Application.Services.CurrentUserService;
using Nowcfo.Domain.Models;
using Nowcfo.Domain.Models.AppUserModels;
using Nowcfo.Infrastructure.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Nowcfo.Infrastructure.Data.ConfigureRelation;

namespace Nowcfo.Infrastructure.Data
{

    public class ApplicationDbContext :IdentityDbContext<AppUser, AppRole, Guid>,IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;

        private const string IsDeletedColumnName = "IsDeleted";
        private const string DeletedByColumnName = "DeletedBy";
        private const string DeletedDateColumnName = "DeletedDate";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<EmployeeInfo>().HasIndex(u => new{ u.Email, u.Phone}).IsUnique();


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).Property<bool>(IsDeletedColumnName).HasDefaultValue(false);
                    modelBuilder.Entity(entityType.ClrType).Property<Guid?>(DeletedByColumnName).IsRequired(false);
                    modelBuilder.Entity(entityType.ClrType).Property<DateTime?>(DeletedDateColumnName).IsRequired(false);
                    modelBuilder.SetSoftDeleteFilter(entityType.ClrType, IsDeletedColumnName);
                }
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>(ConfigureAppUser);
            modelBuilder.Entity<RolePermission>(ConfigureRolePermission);
            modelBuilder.Entity<EmployeeInfo>(ConfigureEmployeeInfo);
            modelBuilder.Entity<Organization>(ConfigureOrganization);
            modelBuilder.Entity<Menu>(ConfigureMenu);
            modelBuilder.Entity<EmployeeInfo>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<EmployeePermission>()
                .HasIndex(e => new { e.EmployeeId, e.LevelOne,e.LevelTwo,e.LevelThree,e.ReferenceId }).IsUnique();

            //modelBuilder.SeedAdminUser();
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            DateTime currentDateTime = DateTime.Now;
            var currentUser = _currentUserService.GetUserId();
            bool isAuthenticateRequest = IsAuthenticatedRequest(currentUser);
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (isAuthenticateRequest)
                            entry.Entity.CreatedBy = currentUser;

                        entry.Entity.CreatedDate = currentDateTime;
                        break;

                    case EntityState.Modified:
                        if (isAuthenticateRequest)
                            entry.Entity.UpdatedBy = currentUser;

                        entry.Entity.UpdatedDate = currentDateTime;
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ISoftDeletableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        if (isAuthenticateRequest)
                            entry.CurrentValues[DeletedByColumnName] = currentUser;

                        entry.CurrentValues[DeletedDateColumnName] = currentDateTime;
                        entry.CurrentValues[IsDeletedColumnName] = true;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public  int SaveChange()
        {
            DateTime currentDateTime = DateTime.Now;
            var currentUser = _currentUserService.GetUserId();
            bool isAuthenticateRequest = IsAuthenticatedRequest(currentUser);
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (isAuthenticateRequest)
                            entry.Entity.CreatedBy = currentUser;

                        entry.Entity.CreatedDate = currentDateTime;
                        break;

                    case EntityState.Modified:
                        if (isAuthenticateRequest)
                            entry.Entity.UpdatedBy = currentUser;

                        entry.Entity.UpdatedDate = currentDateTime;
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ISoftDeletableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        if (isAuthenticateRequest)
                            entry.CurrentValues[DeletedByColumnName] = currentUser;

                        entry.CurrentValues[DeletedDateColumnName] = currentDateTime;
                        entry.CurrentValues[IsDeletedColumnName] = true;
                        break;
                }
            }
            return  base.SaveChanges();
        }

        private bool IsAuthenticatedRequest(Guid currentUser) => currentUser != Guid.Empty;

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<EmployeeInfo> EmployeeInfos { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<EmployeeStatusType> EmployeeStatusTypes { get; set; }
        public DbSet<EmployeePermission> EmployeePermissions { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<DynamicFilterField> DynamicFilterFields { get; set; }
        public DbSet<MarketMaster> MarketMasters { get; set; }
        public DbSet<MarketAllocation> MarketAllocations { get; set; }
        public DbSet<AllocationType> AllocationTypes { get; set; }
        public DbSet<OtherType> OtherTypes { get; set; }
        public DbSet<CogsType> CogsTypes { get; set; }

    }
}
