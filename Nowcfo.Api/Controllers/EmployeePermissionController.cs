using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.IRepository;
using System;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{
    public class EmployeePermissionController : BaseController
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public EmployeePermissionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetEmployeePermissions()
        {
            var permissions = await _unitOfWork.EmployeePermissionRepository.GetAllAsync();
            return Ok(permissions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeePermission(int id)
        {
            try
            {
                var employeePermission = await _unitOfWork.EmployeePermissionRepository.GetByIdAsync(id);
                if (employeePermission == null) return NotFound($"Could not find Employee Permission with id {id}");
                return Ok(employeePermission);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }


        [HttpGet("GetLevelOrganizations/{organizationId}")]
        public async Task<IActionResult> GetLevelOrganizations(int organizationId)
        {
            try
            {
                var permissionTree = await _unitOfWork.EmployeePermissionRepository.GetLevelOrganizations(organizationId);
                return Ok(permissionTree);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.InnerException != null ? e.InnerException?.Message : e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployee(EmployeePermissionDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _unitOfWork.EmployeePermissionRepository.CreateAsync(dto);

                if (await _unitOfWork.SaveChangesAsync())
                    return CreatedAtAction("GetEmployeePermission", new { id = dto.EmployeeId }, dto);
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message, dto);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] EmployeePermissionDto dto)
        {
            try
            {
                dto.EmployeeId = id;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingEmployeePermission = await _unitOfWork.EmployeePermissionRepository.GetByIdAsync(id);
                if (existingEmployeePermission == null)
                    return NotFound($"Could not find Employee with id {id}");

                _unitOfWork.EmployeePermissionRepository.Update(dto);
                if (await _unitOfWork.SaveChangesAsync())
                    return NoContent();
                return BadRequest();

            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

    }
}
