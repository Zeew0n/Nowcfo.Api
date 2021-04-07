using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.Extensions;
using Nowcfo.Application.Helper.Pagination;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet("PaginatedEmployees")]
        public async Task<IActionResult> GetPaginatedEmployees([FromQuery] Param param)
        {
            var emp = await _unitOfWork.EmployeeRepository.GetPagedListAsync(param);
            Response.AddPagination(emp.CurrentPage, emp.PageSize,
              emp.TotalCount, emp.TotalPages);

            return Ok(emp);
        }


        [HttpGet("GetEmployeeTypes")]
        public async Task<IActionResult> GetEmployeeType()
        {
            var enumVals = new List<object>();

            foreach (var item in Enum.GetValues(typeof(EmployeeType)))
            {

                enumVals.Add(new 
                {
                    employeeType = (int)item,
                    name= item.ToString()
                });
            }

            return Ok(enumVals);
        }


        [HttpGet("listallsupervisors")]
        public async Task<IActionResult> GetSuperVisors()
        {
            var emp = await _unitOfWork.EmployeeRepository.GetAllSuperVisors();
            return Ok(emp);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
                if (employee == null) return NotFound($"Could not find Employee with id {id}");
                return Ok(employee);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployee(EmployeeInfoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _unitOfWork.EmployeeRepository.CreateAsync(dto);

                if (await _unitOfWork.SaveChangesAsync())
                    return CreatedAtAction("GetEmployee", new { id = dto.EmployeeId }, dto);
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message, dto);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] EmployeeInfoDto dto)
        {
            try
            {
                dto.EmployeeId = id;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingEmployee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
                if (existingEmployee == null)
                    return NotFound($"Could not find Employee with id {id}");

                // _mapper.Map(dto, existingEmployee);

                _unitOfWork.EmployeeRepository.Update(dto);
                if (await _unitOfWork.SaveChangesAsync())
                    return NoContent();
                return BadRequest();

            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var existingEmployee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
                if (existingEmployee == null)
                    return NotFound($"Could not find Employee with id {id}");
                _unitOfWork.EmployeeRepository.Delete(existingEmployee);
                if (await _unitOfWork.SaveChangesAsync())
                    return NoContent();
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        [HttpGet("NonPaginatedEmployees")]
        public async Task<IActionResult> GetNonPaginatedEmployees()
        {
            var emp = await _unitOfWork.EmployeeRepository.GetAllAsync();

            return Ok(emp);
        }

        [HttpGet("EmployeesAutocomplete/{searchText}")]
        public async Task<IActionResult> GetEmployeesAutocomplete(  string searchText)
        {
            try
            {
                var employees = await _unitOfWork.EmployeeRepository.GetEmployeesAutocompleteAsync(searchText);
                return Ok(employees);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        [HttpPut("AssignEmployee")]
        public async Task<IActionResult> PutAssignEmployee([FromBody] AssignEmployeeDto dto)
        {
            try
            {
                var existingEmployee = await _unitOfWork.EmployeeRepository.GetByIdAsync(dto.EmployeeId);
                if (existingEmployee == null)
                    return NotFound($"Could not find Employee with id {dto.EmployeeId}");
                existingEmployee.OrganizationId = dto.OrganizationId;
                var employeeDto = _mapper.Map<EmployeeInfoDto>(existingEmployee);
                _unitOfWork.EmployeeRepository.Update(employeeDto);
                if (await _unitOfWork.SaveChangesAsync())
                    return NoContent();
                return BadRequest();

            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        //
        [HttpGet("SyncHierarchy")]
        public async Task<IActionResult> GetSyncHierarchy()
        {
            try
            {
                var organization = await _unitOfWork.EmployeeRepository.GetSyncFusionOrganizations();
                return Ok(organization);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.InnerException != null ? e.InnerException?.Message : e.Message);
            }
        }

        [HttpGet("EmployeePermission/{employeeId}")]
        public async Task<IActionResult> GetEmployeePermissionHierarchy(int employeeId)
        {
            try
            {
                var permissionTree = await _unitOfWork.EmployeeRepository.GetEmployeePermissionHierarchy(employeeId);
                return Ok(permissionTree);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.InnerException != null ? e.InnerException?.Message : e.Message);
            }
        }


        [HttpGet("CheckedPermission/{employeeId}")]
        public async Task<IActionResult> GetCheckedPermissions(int employeeId)
        {
            try
            {
                var permissionTree = await _unitOfWork.EmployeeRepository.GetCheckedPermissions(employeeId);
                return Ok(permissionTree);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.InnerException != null ? e.InnerException?.Message : e.Message);
            }
        }
    }
}