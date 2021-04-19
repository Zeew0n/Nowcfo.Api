using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.Dtos.User.Response;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models;
using Nowcfo.Domain.Models.AppUserModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nowcfo.Application.Services.CurrentUserService;

namespace Nowcfo.Application.Repository
{
    public class MenuRepository:IMenuRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public MenuRepository(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<MenuDto> GetByIdAsync(Guid id)
        {
            try
            {
                var menu = await _dbContext.Menus.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
                return _mapper.Map<MenuDto>(menu);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<List<MenuDto>> GetAllAsync()
        {
            try
            {
                var menus = await _dbContext.Menus .ToListAsync();
                return _mapper.Map<List<MenuDto>>(menus);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task CreateAsync(Menu model)
        {
            try
            {
                await _dbContext.Menus.AddAsync(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Update(Menu model)
        {
            try
            {
                _dbContext.Menus.Update(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Delete(Menu model)
        {
            try
            {
                _dbContext.Menus.Remove(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<List<MenuDto>> GetMenusByUserRoleAsync(Guid userId)
        {
            try
            {
                var userDetails =
                                      await(from user in _dbContext.AppUsers
                                            join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                                            join role in _dbContext.AppRoles on userRole.RoleId equals role.Id
                                            join rolePer in _dbContext.RolePermissions on role.Id equals rolePer.RoleId into rp
                                            from rolePermission in rp.DefaultIfEmpty()
                                            join perm in _dbContext.Permissions on rolePermission.PermissionId equals perm.Id into per
                                            from permission in per.DefaultIfEmpty()

                                            join men in _dbContext.Menus on permission.MenuId equals men.Id into menus
                                            from menu in menus.DefaultIfEmpty()


                                            where user.Id == userId
                                            select new
                                            {
                                                user.IsAdmin,
                                                RoleId = role.Id,
                                                RoleName = role.Name,
                                                Menu = menu,
                                                Permission = permission == null ? null : permission.Slug,
                                              
                                            }).OrderBy(t => t.Permission).ToListAsync();

                var userDto = userDetails.GroupBy(t => t.RoleId)
                .Select(q =>
                {
                    return new AppUserDto
                    {
                        IsAdmin = q.Select(t => t.IsAdmin).FirstOrDefault(),
                        RoleId = q.Key,
                        RoleName = q.Select(t => t.RoleName).FirstOrDefault(),
                        Permissions = q.Where(t => t.Permission != null).Select(t => t.Permission).Distinct().ToList(),
                        AssignedMenus = _mapper.Map<List<MenuDto>>(q.Where(t => t.Menu != null).Select(x => x.Menu).DistinctBy(x => x.MenuName).OrderBy(x => x.DisplayOrder)),
                    };
                }).FirstOrDefault();

                if (userDto != null && userDto.IsAdmin)
                    userDto.AssignedMenus = _mapper.Map<List<MenuDto>>(_dbContext.Menus.Where(m => m.MenuLevel == 1)
                        .OrderBy(x => x.DisplayOrder).ToList());

                return userDto?.AssignedMenus;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
