using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.Extensions;
using Nowcfo.Application.Helper.Pagination;
using Nowcfo.Application.IRepository;
using System;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{

    public class MarketMasterController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public MarketMasterController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [HttpGet("PaginatedAllocation")]
        public async Task<IActionResult> GetPaginatedAllocation([FromQuery] ParamMarket param)
        {
            var emp = await _unitOfWork.MarketAllocationRepository.GetPagedListAsync(param);
            Response.AddPagination(emp.CurrentPage, emp.PageSize,
              emp.TotalCount, emp.TotalPages);

            return Ok(emp);
        }


        [HttpGet("listallorganizations")]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var emp = await _unitOfWork.MarketAllocationRepository.GetAllOrganizations();
            return Ok(emp);
        }


        [HttpGet("GetMarketAllocationList/{id}")]
        public async Task<IActionResult> GetMarketAllocationList(int id)
        {
            try
            {
                var allocations = await _unitOfWork.MarketAllocationRepository.GetAllMarketsByOrgIdXXX(id);
                return Ok(allocations);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.InnerException != null ? e.InnerException?.Message : e.Message);
            }
        }



        [HttpGet("GetAllocationTypes")]
        public async Task<IActionResult> GetAllocationTypes()
        {
            var emp = await _unitOfWork.MarketAllocationRepository.GetAllocationTypes();
            return Ok(emp);
        }

        [HttpGet("GetCogsTypes")]
        public async Task<IActionResult> GetCogsTypes()
        {
            var emp = await _unitOfWork.MarketAllocationRepository.GetCogsTypes();
            return Ok(emp);
        }

        [HttpGet("GetOtherTypes")]
        public async Task<IActionResult> GetOtherTypes()
        {
            var emp = await _unitOfWork.MarketAllocationRepository.GetOtherTypes();
            return Ok(emp);
        }

        [HttpGet("GetMarketAllocationListByOrgId/{id}")]
        public async Task<IActionResult> GetAllMarketsByOrgId(int id)
        {
            try
            {
                var allocations = await _unitOfWork.MarketAllocationRepository.GetAllMarketList(id);
                return Ok(allocations);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.InnerException != null ? e.InnerException?.Message : e.Message);
            }
        }



        [HttpGet("GetMarketAllocationList/{masterId}/{payPeriod}/{id}/{allocationTypeId}")]
        public async Task<IActionResult> GetAllAllocationsById(int masterId, string payPeriod, int id, int allocationTypeId)
        {
            try
            {
                var allocations = await _unitOfWork.MarketAllocationRepository.GetAllAllocationsById(masterId,payPeriod,id,allocationTypeId);
                return Ok(allocations);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.InnerException != null ? e.InnerException?.Message : e.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarketMaster(int id)
        {
            try
            {
                var allocation = await _unitOfWork.MarketAllocationRepository.GetByIdAsync(id);
                if (allocation == null) return NotFound($"Could not find Master Allocation with id {id}");
                return Ok(allocation);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarketMaster([FromRoute] int id, [FromBody] MarketMasterDto dto)
        {
            try
            {
                dto.Id = id;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingMaster = await _unitOfWork.MarketAllocationRepository.GetByIdAsync(id);
                if (existingMaster == null)
                    return NotFound($"Could not find Employee with id {id}");


                _unitOfWork.MarketAllocationRepository.Update(dto);
                if (await _unitOfWork.SaveChangesAsync())
                    return NoContent();
                return BadRequest();

            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> PostMarketMaster(MarketMasterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                await _unitOfWork.MarketAllocationRepository.CreateAsync(dto);
                if (await _unitOfWork.SaveChangesAsync())
                    return Ok();
                //return CreatedAtAction("GetMarketAllocation", new { id = dto.Id }, dto);
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message, dto);
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarketMaster(int id)
        {
            try
            {
                var existingAllocation = await _unitOfWork.MarketAllocationRepository.GetByIdAsync(id);
                if (existingAllocation == null)
                    return NotFound($"Could not find Master Allocation with id {id}");
                _unitOfWork.MarketAllocationRepository.Delete(existingAllocation);
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
