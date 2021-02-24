using System;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        IOrganizationRepository OrganizationRepository { get;}
        IEmployeeRepository EmployeeRepository { get; }
        IUserRepository UserRepository { get;}
        IRoleRepository RoleRepository { get;}
        IUserRoleRepository UserRoleRepository { get; }
        IUserClaimRepository UserClaimRepository { get; }
        IRolePermissionRepository RolePermissionRepository { get; }
        IRolePermissionMappingRepository RolePermissionMappingRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }

        
        void BeginTransaction();

        void Commit();

        void RollBack();
        Task<bool> SaveChangesAsync();

        //int SaveChanges();

        //Task<int> SaveChangesAsync();
    }
}
