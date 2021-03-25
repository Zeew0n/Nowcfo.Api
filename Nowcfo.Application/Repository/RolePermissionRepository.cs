using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models.AppUserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.Application.Repository
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public RolePermissionRepository(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<RolePermissionDto> GetByIdAsync(Guid id)
        {
            try
            {
                var rolePermission = await(from r in _dbContext.AppRoles
                    join rp in _dbContext.RolePermissions on r.Id equals rp.RoleId into rpg
                    from grp in rpg.DefaultIfEmpty()
                    join p in _dbContext.Permissions on grp.PermissionId equals p.Id into pg
                    from gp in pg.DefaultIfEmpty()
                    join m in _dbContext.Menus on gp.MenuId equals m.Id into mg
                    from gm in mg.DefaultIfEmpty()
                    where r.Id == id
                    select new
                    {
                        roleId = r.Id,
                        roleName = r.Name,
                        permissionId = grp == null ? default(Guid) : grp.PermissionId,
                        MenuId = gm == null ? default(Guid) : gm.Id
                    }).ToListAsync();

                    return  rolePermission.GroupBy(x => x.roleId)
                    .Select(x => new RolePermissionDto
                    {
                        RoleId = x.Select(y => y.roleId).FirstOrDefault(),
                        RoleName = x.Select(y => y.roleName).FirstOrDefault(),
                        PermissionIds = x.Select(y => y.permissionId).Distinct().ToList(),
                        MenuIds = x.Select(y => y.MenuId).Distinct().ToList()
                    }).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task<List<RolePermissionDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(RolePermission model)
        {
            throw new NotImplementedException();
        }

        public void Update(RolePermission model)
        {
            throw new NotImplementedException();
        }

        public void Delete(RolePermission model)
        {
            throw new NotImplementedException();
        }
    }
}