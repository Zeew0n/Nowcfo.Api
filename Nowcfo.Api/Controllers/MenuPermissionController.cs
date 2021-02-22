using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Domain.Models;
using Nowcfo.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuPermissionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MenuPermissionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MenuPermission
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuPermission>>> GetMenuPermissions()
        {
            return await _context.MenuPermissions.ToListAsync();
        }

        // GET: api/MenuPermission/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuPermission>> GetMenuPermission(string id)
        {
            var menuPermission = await _context.MenuPermissions.FindAsync(id);

            if (menuPermission == null)
            {
                return NotFound();
            }

            return menuPermission;
        }

        // PUT: api/MenuPermission/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenuPermission(string id, MenuPermission menuPermission)
        {
            if (id != menuPermission.MenuPermissionId)
            {
                return BadRequest();
            }

            _context.Entry(menuPermission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuPermissionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MenuPermission
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MenuPermission>> PostMenuPermission(MenuPermission menuPermission)
        {
            _context.MenuPermissions.Add(menuPermission);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MenuPermissionExists(menuPermission.MenuPermissionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMenuPermission", new { id = menuPermission.MenuPermissionId }, menuPermission);
        }

        // DELETE: api/MenuPermission/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuPermission(string id)
        {
            var menuPermission = await _context.MenuPermissions.FindAsync(id);
            if (menuPermission == null)
            {
                return NotFound();
            }

            _context.MenuPermissions.Remove(menuPermission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MenuPermissionExists(string id)
        {
            return _context.MenuPermissions.Any(e => e.MenuPermissionId == id);
        }
    }
}
