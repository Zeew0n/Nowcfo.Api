using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.API.Attributes;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.Dtos.Role;
using Nowcfo.Application.Helper;
using Nowcfo.Application.IRepository;
using Nowcfo.Application.Services.RoleService;
using Nowcfo.Domain.Models;
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
        //[Permission(CrudPermission.AddRole)]
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
        //[Permission(CrudPermission.ViewRole)]
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
        //[Permission(CrudPermission.ViewRole)]
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
        [Permission(CrudPermission.UpdateRole)]
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
        //[Permission(CrudPermission.DeleteRole)]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                await _roleService.DeleteAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpGet("menus")]
        //[Permission(CrudPermission.DeleteRole)]
        public async Task<IActionResult> GetAllMenus()
        {
            try
            {
                var permissions = await _roleService.GetAllMenus();
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPost("CreateRolePermission")]
        public async Task<IActionResult> PostRolePermission(RolePermissionDto dto)
        {
            try
            {
                await _roleService.AddRolePermission(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }

        [HttpPut("UpdateRolePermission")]
        public async Task<IActionResult> PutRolePermission(RolePermissionDto dto)
        {
            try
            { 
                await _roleService.EditRolePermission(dto);
                await _roleService.AddRolePermission(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
            }
        }
    }
}