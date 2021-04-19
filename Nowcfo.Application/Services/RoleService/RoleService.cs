using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.Dtos.Role;
using Nowcfo.Application.Exceptions;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models.AppUserModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.Application.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _dbContext;
        public RoleService(IApplicationDbContext context,UserManager<AppUser> userManager, RoleManager<AppRole> roleMgr, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _roleManager = roleMgr;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dbContext = context;
        }

        /// <summary>
        /// Creates a new role and assigns role access asynchronously.
        /// </summary>
        /// <param name="appRole">roleName and roleAccess list</param>
        /// <returns></returns>
        public async Task CreateAsync(AppRole appRole)
        {
            try
            {
                
                await _roleManager.CreateAsync(appRole);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<IdentityResult> AddToRoleAsync(AppUser userIdentity, string role)
        {
            try
            {
                var result = await _userManager.AddToRoleAsync(userIdentity, role);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(AppUser appUser)
        {
            try
            {
                var roleList = await GetUserRolesAsync(appUser);
                return await _userManager.RemoveFromRolesAsync(appUser, roleList);
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private async Task<IList<string>> GetUserRolesAsync(AppUser appUser) =>
        await _userManager.GetRolesAsync(appUser);

        public async Task<string> GetRoleNameByIdAsync(Guid roleId)
        {
            try
            {
                var result = await _roleManager.Roles.Where(q => q.Id == roleId).Select(t=>t.Name).SingleOrDefaultAsync();
                if (string.IsNullOrEmpty(result))
                    throw new ApiException("Provided role doesn't exist");

                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        //

        public async Task<RoleDto> CreateAsync(RoleDto model)
        {
            try
            {
                AppRole appRole = new AppRole
                {
                    Name = model.RoleName,
                    NormalizedName = model.RoleName.ToUpper()
                };

                await _roleManager.CreateAsync(appRole);

                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<AppRole, RoleDto>(appRole);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }


        }



        public async Task<bool> DeleteAsync(Guid Id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(Id.ToString());
                if (role != null)
                {
                    IdentityResult result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                        return true;
                    return false;
                }
                else
                {
                    Log.Error("Error: Couldn't find Role code  {id} id", Id);
                    return false;
                }
                
               
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;

            }
        }

        public async Task<List<RoleDto>> GetAllAsync(Guid? id = null)
        {
            try
            {
                var res = await _roleManager.Roles.ToListAsync();
                return _mapper.Map<List<RoleDto>>(res);
                
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async  Task<RoleDto> GetByIdAsync(Guid id)
        {
            try
            {
                  return  await _roleManager.Roles.Where(r=>r.Id==id).Select(x => new RoleDto
                  {
                      RoleId = x.Id,
                      RoleName = x.Name


                  }).FirstOrDefaultAsync();
                
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(RoleDto model)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                
                role.Name = model.RoleName ?? role.Name;
                role.NormalizedName = model.RoleName.ToUpper() ?? role.Name.ToUpper();

                await _roleManager.UpdateAsync(role);
                await _unitOfWork.SaveChangesAsync();


                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
        private async Task<bool> RolePremission(RoleDto model)
        {
            try
            {


                var roles = await _dbContext.RolePermissions.Where(x => x.RoleId == model.RoleId).ToListAsync();
                
                var rolePermissionMapping = new List<RolePermission>();
                if (model.PermissionIds != null)
                {
                    foreach (var role in model.PermissionIds)
                    {
                        var permission = new RolePermission();

                        permission.PermissionId = role;
                        permission.RoleId = model.RoleId;
                        rolePermissionMapping.Add(permission);

                    }
                    if (roles != null)
                    {
                        
                        //_dbContext.RolePermission.RemoveRange(roles);

                        //_unitOfWork.RolePermissionMappingRepository.Remove(roles);

                        //_unitOfWork.RolePermissionMappingRepository.Add(rolePermissionMapping);

                    }

                }
                else
                {
                   // _unitOfWork.RolePermissionMappingRepository.Remove(roles);
                }


                return true;
            }

            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
        public async Task<List<MenuDto>> GetParentMenusForPermission()
        {
            try
            {
                var menus = await _dbContext.Menus.Where(x=>x.MenuLevel==1).ToListAsync();
                return _mapper.Map<List<MenuDto>>(menus);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

       
        public async Task AddRolePermission(RolePermissionDto dto)
        {
            try
            {
                var permissionIds = _dbContext.Permissions.Where(x => dto.MenuIds.Contains(x.MenuId)).Select(x => x.Id);

                foreach (var permId in permissionIds)
                {
                    var rolePermission = new RolePermission
                    {
                        RoleId = dto.RoleId,
                        PermissionId = permId
                    };
                     await _dbContext.RolePermissions.AddAsync(rolePermission);
                }
                _dbContext.SaveChange();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task EditRolePermission(RolePermissionDto dto)
        {
            try
            {
                var rolePermissions = await _dbContext.RolePermissions.Where(x => x.RoleId == dto.RoleId).ToListAsync();
                _dbContext.RolePermissions.RemoveRange(rolePermissions);
                _dbContext.SaveChange();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
