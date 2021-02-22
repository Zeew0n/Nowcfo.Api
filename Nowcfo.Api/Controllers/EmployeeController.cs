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
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeInfo>>> GetEmployeeInfos()
        {
            return await _context.EmployeeInfos.ToListAsync();
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeInfo>> GetEmployeeInfo(int id)
        {
            var employeeInfo = await _context.EmployeeInfos.FindAsync(id);

            if (employeeInfo == null)
            {
                return NotFound();
            }

            return employeeInfo;
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeInfo(int id, EmployeeInfo employeeInfo)
        {
            if (id != employeeInfo.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employeeInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeInfoExists(id))
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

        // POST: api/Employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeInfo>> PostEmployeeInfo(EmployeeInfo employeeInfo)
        {
            _context.EmployeeInfos.Add(employeeInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeInfo", new { id = employeeInfo.EmployeeId }, employeeInfo);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeInfo(int id)
        {
            var employeeInfo = await _context.EmployeeInfos.FindAsync(id);
            if (employeeInfo == null)
            {
                return NotFound();
            }

            _context.EmployeeInfos.Remove(employeeInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeInfoExists(int id)
        {
            return _context.EmployeeInfos.Any(e => e.EmployeeId == id);
        }
    }
}
