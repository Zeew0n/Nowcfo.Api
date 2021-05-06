using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Nowcfo.Application.IRepository;
using Nowcfo.Application.Repository;
using Nowcfo.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Nowcfo.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        //private readonly IConfiguration _config;

        private readonly ApplicationDbContext _dbContext;
        public IOrganizationRepository OrganizationRepository { get; }
        public IDesignationRepository DesignationRepository { get; set; }
        public IEmployeeRepository EmployeeRepository { get; }
        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IUserClaimRepository UserClaimRepository { get; }
        public IRolePermissionRepository RolePermissionRepository { get; }
        public IMenuRepository MenuRepository { get; }
        public IEmployeePermissionRepository EmployeePermissionRepository { get; }
        public IMarketAllocationRepository MarketAllocationRepository { get; }
        public IDapperRepository DapperRepository { get; }


        public UnitOfWork(ApplicationDbContext context,IDapperRepository dapper,IConfiguration config, IMapper mapper )
        {
            _dbContext = context;
            OrganizationRepository = new OrganizationRepository(context,mapper);
            UserRepository = new UserRepository(context,mapper);
            DesignationRepository = new DesignationRepository(context, mapper);
            EmployeeRepository = new EmployeeRepository(context, mapper);
            RolePermissionRepository = new RolePermissionRepository(context, mapper);
            MenuRepository =new MenuRepository(context,mapper);
            EmployeePermissionRepository = new EmployeePermissionRepository(context, mapper);
            MarketAllocationRepository = new MarketAllocationRepository(context, mapper,dapper);
            DapperRepository = new DapperRepository(config);

        }


        public void BeginTransaction()
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(UnitOfWork).Name);

            if (_transaction == null)
                _transaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            finally
            {
                if (_transaction == null)
                    _transaction = null;
            }
        }

        public void RollBack()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                if (_transaction == null)
                    _transaction = null;
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<bool> SaveChangesAsync() => await _dbContext.SaveChangesAsync()>=0;
    }
}
