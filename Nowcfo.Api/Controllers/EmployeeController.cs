using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.Application.DTO;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("listallemployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var emp = await _unitOfWork.EmployeeRepository.GetAllAsync();
            return Ok(emp);
        }

        [HttpGet("getemployee/{id}")]
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
        [HttpPost("create")]

        public async Task<IActionResult> PostEmployee(EmployeeInfoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var employee = _mapper.Map<EmployeeInfo>(dto);
                await _unitOfWork.EmployeeRepository.CreateAsync(employee);
                if (await _unitOfWork.SaveChangesAsync())
                    return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, dto);
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message, dto);
            }
        }

        // [HttpPut("{id}")]
        [HttpPut("update/{id}")]

        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] EmployeeInfoDto dto)
        {
            try
            {
                dto.EmployeeId = id;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingEmployee = _mapper.Map<EmployeeInfo>(await _unitOfWork.EmployeeRepository.GetByIdAsync(id));
                if (existingEmployee == null)
                    return NotFound($"Could not find Employee with id {id}");

                _mapper.Map(dto, existingEmployee);

                _unitOfWork.EmployeeRepository.Update(existingEmployee);
                if (await _unitOfWork.SaveChangesAsync())
                    return NoContent();
                return BadRequest();

            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        //[HttpDelete("{id}")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var existingEmployee = _mapper.Map<EmployeeInfo>(await _unitOfWork.EmployeeRepository.GetByIdAsync(id));
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