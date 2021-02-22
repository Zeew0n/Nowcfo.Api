using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.Application.Dtos.Role;
using Nowcfo.Application.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{
    public class RoleController : BaseController
    {
      
        private readonly IUnitOfWork _unitOfWork;
        public RoleController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create")]
        //[AllowAnonymous]
       // [Permission(Permission.AddRole)]
        public async Task<IActionResult> Create([FromBody] RoleDto role)
        {
            var query = await _unitOfWork.RoleRepository.CreateAsync(role);
            return Ok(query);
        }

        [HttpGet("Listrole")]
        //[Permission(Permission.ViewRole)]
        public async Task<IActionResult> Read()
        {
            try
            {
                var result = await _unitOfWork.RoleRepository.GetAllAsync();
                
                if (!result.Any())
                {
                    return NotFound(HandleActionResult("No data available", StatusCodes.Status404NotFound));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpGet("Role/{Id}")]
        //[Permission(Permission.ViewRole)]
        public async Task<IActionResult> Read(Guid Id)
        {
            try
            {
                var result = await _unitOfWork.RoleRepository.GetByIdAsync(Id);
                if (result == null)
                {
                    return NotFound(HandleActionResult("No data available", StatusCodes.Status404NotFound));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("UpdateRole")]
       // [Permission(Permission.UpdateRole)]
       // [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] RoleDto actionType)
        {
            try
            {

               
                var result = await _unitOfWork.RoleRepository.UpdateAsync(actionType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpDelete("deleterole/{Id}")]
       // [Permission(Permission.DeleteRole)]
        //[AllowAnonymous]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                if (Id != null)
                {
                    await _unitOfWork.RoleRepository.DeleteAsync(Id);
                    return Ok();
                }
                else
                {
                    return BadRequest(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }
    }
}