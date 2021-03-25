using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.IRepository;
using System;
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
            var emp = await _unitOfWork.EmployeeRepository.GetAllAsync();
            return Ok(emp);
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

        //[HttpPost]
        [HttpPost]

        public async Task<IActionResult> PostEmployee(EmployeeUpdateDto dto)
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

        // [HttpPut("{id}")]
        [HttpPut("{id}")]

        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] EmployeeUpdateDto dto)
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
        [AllowAnonymous]
        [HttpGet("listallpermissions/{employeeId}")]

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



        [HttpGet("KendoHierarchy")]
        [AllowAnonymous]
        public async Task<IActionResult> GetKendoHierarchy()
        {
            try
            {
                var organizationTree = await _unitOfWork.EmployeeRepository.GetKendoTreeHierarchy();
                return Ok(organizationTree);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.InnerException != null ? e.InnerException?.Message : e.Message);
            }
        }



        //[HttpDelete("{id}")]
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
    }
}