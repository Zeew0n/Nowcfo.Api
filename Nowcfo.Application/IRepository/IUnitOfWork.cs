﻿using System;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        IOrganizationRepository OrganizationRepository { get;}
        IDesignationRepository DesignationRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IUserRepository UserRepository { get;}
        IRoleRepository RoleRepository { get;}
        IUserRoleRepository UserRoleRepository { get; }
        IUserClaimRepository UserClaimRepository { get; }
        IRolePermissionRepository RolePermissionRepository { get; }
        IMenuRepository MenuRepository { get;}
        IEmployeePermissionRepository EmployeePermissionRepository { get; }
        IMarketAllocationRepository MarketAllocationRepository { get; }
        IDapperRepository DapperRepository { get; }
        ISalesForecastRepository SalesForecastRepository { get; }



        void BeginTransaction();

        void Commit();

        void RollBack();
        Task<bool> SaveChangesAsync();

    }
}
