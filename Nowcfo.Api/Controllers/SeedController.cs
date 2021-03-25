using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.API.Controllers.Base;
using Nowcfo.API.Helpers;
using Nowcfo.Domain.Models.AppUserModels;
using Nowcfo.Infrastructure.Data;
using Nowcfo.Infrastructure.Data.Seed;
using System;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{
    [AllowAnonymous]
    [Route("api/InitialSeed")]
    public class SeedController: BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public SeedController(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("UsersRoles")]
        public async Task<IActionResult> SeedUsersRoles()
        {
            try
            {
                if(await DbInitializer.InitializeUserRolesAsync(_context, _userManager, _roleManager))
                    return Ok("Seeded UsersRoles Successfully");
                else
                    return BadRequest($"Failed to seed UsersRoles");
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        [HttpPost("Permission")]
        public async Task<IActionResult> SeedPermissions()
        {
            try
            {
                if (await MenuData.SeedMenus(_context))
                {
                    if (await PermissionData.SeedPermissions(_context))
                        return Ok("Seeded Menus and Permission Successfully");
                    else
                        return BadRequest($"Failed to seed Permission");
                }
                else
                {
                    return BadRequest($"Failed to seed Menus");
                }
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        [HttpPost("RolePermission")]
        public async Task<IActionResult> SeedRolePermission()
        {
            try
            {
                if (await RolePermissionData.SeedPermissionsForRole(_context))
                    return Ok("seeded RolePermission Successfully");
                else
                    return BadRequest($"Failed to seed RolePermission");
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }
    }
}
