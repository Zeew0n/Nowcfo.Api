using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.Application.Dtos.Role;
using Nowcfo.Application.IRepository;
using Nowcfo.Application.Services.RoleService;
using Nowcfo.Domain.Models.AppUserModels;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace Nowcfo.API.Controllers
{
    public class RoleController : BaseController
    {
      
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        public RoleController(IUnitOfWork unitOfWork, IRoleService roleService, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpPost("Create")]
        //[AllowAnonymous]
       // [Permission(Permission.AddRole)]
        public async Task<IActionResult> Create([FromBody] RoleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var role = _mapper.Map<AppRole>(dto);
                await _roleService.CreateAsync(role);

                if (await _unitOfWork.SaveChangesAsync())
                    return CreatedAtAction("Read", new { id = role.Id }, dto);
                return BadRequest();
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex.Message);
            }
        }

        [HttpGet("Listrole")]
        //[Permission(Permission.ViewRole)]
        public async Task<IActionResult> Read()
        {
            try
            {
                var result = await _roleService.GetAllAsync();
                
                if (!result.Any())
                {
                    return NotFound("No data available");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex.Message);
            }
        }

        [HttpGet("Role/{Id}")]
        //[Permission(Permission.ViewRole)]
        public async Task<IActionResult> Read(Guid Id)
        {
            try
            {
                var result = await _roleService.GetByIdAsync(Id);
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

               
                var result = await _roleService.UpdateAsync(actionType);
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
                    await _roleService.DeleteAsync(Id);
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