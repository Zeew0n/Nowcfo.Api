﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.Application.DTO;
using Nowcfo.Domain.Models;
using System;
using System.Threading.Tasks;
using Nowcfo.Application.IRepository;
using Nowcfo.Infrastructure.Repository;

namespace Nowcfo.API.Controllers
{
    public class OrganizationController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrganizationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrganizations()
        {
            var org = await _unitOfWork.OrganizationRepository.GetAllAsync();
            return Ok(org);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganization(int id)
        {
            try
            {
                var organization = await _unitOfWork.OrganizationRepository.GetByIdAsync(id);
                if (organization == null) return NotFound($"Could not find Organization with id {id}");
                return Ok(organization);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostOrganization(OrganizationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var organization = _mapper.Map<Organization>(dto);
                await _unitOfWork.OrganizationRepository.CreateAsync(organization);

                if (await _unitOfWork.SaveChangesAsync())
                    return CreatedAtAction("GetOrganization", new {id = organization.OrganizationId}, dto);
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message, dto);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganization([FromRoute] int id, [FromBody] OrganizationDto dto)
        {
            try
            {
                dto.OrganizationId = id;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingOrganization = _mapper.Map<Organization>( await _unitOfWork.OrganizationRepository.GetByIdAsync(id));
                if (existingOrganization == null)
                    return NotFound($"Could not find Organization with id {id}");

                _mapper.Map(dto, existingOrganization);

                _unitOfWork.OrganizationRepository.Update(existingOrganization);
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
        public async Task<IActionResult> DeleteOrganization(int id)
        {
            try
            {
                var existingOrganization = _mapper.Map<Organization>(await _unitOfWork.OrganizationRepository.GetByIdAsync(id));
                if (existingOrganization == null)
                    return NotFound($"Could not find Organization with id {id}");

                _unitOfWork.OrganizationRepository.Delete(existingOrganization);
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