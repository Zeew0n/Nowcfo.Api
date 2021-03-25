using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models.AppUserModels;
using System;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{
    public class RolePermissionController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RolePermissionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        
        [HttpGet("ReadAllRolePermission")]
        public async Task<IActionResult> GetRolePermissions()
        {
            var rolePermissions = await _unitOfWork.RolePermissionRepository.GetAllAsync();
            return Ok(rolePermissions);
        }

        
        [HttpGet("ReadRolePermission/{id}")]
        public async Task<IActionResult> GetRolePermission(Guid id)
        {
            try
            {
                var rolePermission = await _unitOfWork.RolePermissionRepository.GetByIdAsync(id);
                if (rolePermission == null) return NotFound();
                return Ok(rolePermission);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }

        }


        [HttpPut("UpdateRolePermission/{id}")]
        public async Task<IActionResult> PutRolePermission([FromRoute] Guid id,[FromBody]RolePermissionDto dto)
        {
            try
            {
                dto.RoleId = id;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingOrganization = _mapper.Map<RolePermission>(await _unitOfWork.RolePermissionRepository.GetByIdAsync(id));
                if (existingOrganization == null)
                    return NotFound($"Could not find RolePer with id {id}");

                _mapper.Map(dto, existingOrganization);

                _unitOfWork.RolePermissionRepository.Update(existingOrganization);
                if (await _unitOfWork.SaveChangesAsync())
                    return NoContent();
                return BadRequest();

            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

   
        [HttpPost("CreateRolePermission")]
        public async Task<IActionResult> PostRolePermission(RolePermissionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var organization = _mapper.Map<RolePermission>(dto);
                await _unitOfWork.RolePermissionRepository.CreateAsync(organization);

                if (await _unitOfWork.SaveChangesAsync())
                    return CreatedAtAction("GetRolePermission", new { id = organization.RoleId }, dto);
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message, dto);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolePermission(Guid id)
        {
            try
            {
                var existingOrganization = _mapper.Map<RolePermission>(await _unitOfWork.RolePermissionRepository.GetByIdAsync(id));
                if (existingOrganization == null)
                    return NotFound($"Could not find Organization with id {id}");

                _unitOfWork.RolePermissionRepository.Delete(existingOrganization);
                if (await _unitOfWork.SaveChangesAsync())
                    return NoContent();
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.InnerException != null ? e.InnerException?.Message : e.Message);
            }
        }

       


        //[HttpPost("CreateRolePermission")]
        //public async Task<IActionResult> PostRolePermission(RolePermissionDto dto)
        //{
        //    try
        //    {
        //        await _roleService.AddRolePermission(dto);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
        //    }
        //}

        //[HttpPost("UpdateRolePermission")]
        //public async Task<IActionResult> PutRolePermission(RolePermissionDto dto)
        //{
        //    try
        //    {
        //        await _roleService.EditRolePermission(dto);
        //        await _roleService.AddRolePermission(dto);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(HandleActionResult(ex.Message, StatusCodes.Status400BadRequest));
        //    }
        //}
    }
}
