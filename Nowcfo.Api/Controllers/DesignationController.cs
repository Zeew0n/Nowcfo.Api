using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{
    public class DesignationController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DesignationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignations()
        {
            var deg = await _unitOfWork.DesignationRepository.GetAllAsync();
            return Ok(deg);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDesignation(int id)
        {
            try
            {
                var designation = await _unitOfWork.DesignationRepository.GetByIdAsync(id);
                if (designation == null) return NotFound($"Could not find Designation with id  {id}");
                return Ok(designation);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostDesignation(DesignationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var designation = _mapper.Map<Designation>(dto);
                await _unitOfWork.DesignationRepository.CreateAsync(designation);

                if (await _unitOfWork.SaveChangesAsync())
                    return CreatedAtAction("GetDesignation", new { id = designation.DesignationId }, dto);
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message, dto);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDesignation([FromRoute] int id, [FromBody] DesignationDto dto)
        {
            try
            {
                dto.DesignationId = id;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingDesignation = _mapper.Map<Designation>(await _unitOfWork.DesignationRepository.GetByIdAsync(id));
                if (existingDesignation == null)
                    return NotFound($"Could not find Designation with id {id}");

                _mapper.Map(dto, existingDesignation);

                _unitOfWork.DesignationRepository.Update(existingDesignation);
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
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            try
            {
                var existingDesignation = _mapper.Map<Designation>(await _unitOfWork.DesignationRepository.GetByIdAsync(id));
                if (existingDesignation == null)
                    return NotFound($"Could not find Designation with id {id}");

                _unitOfWork.DesignationRepository.Delete(existingDesignation);
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