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

    public class SalesForecastsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public SalesForecastsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;

        }


        [HttpGet("PaginatedForecasts")]
        public async Task<IActionResult> GetPaginatedForecasts([FromQuery] Param param)
        {
            var forcast = await _unitOfWork.SalesForecastRepository.GetPagedListAsync(param);
            Response.AddPagination(forcast.CurrentPage, forcast.PageSize,
              forcast.TotalCount, forcast.TotalPages);
            return Ok(forcast);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalesForecasts(int id)
        {
            try
            {
                var forecast = await _unitOfWork.SalesForecastRepository.GetByIdAsync(id);
                if (forecast == null) return NotFound($"Could not find Forecast with id {id}");
                return Ok(forecast);
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }


        

        [HttpPost]
        public async Task<IActionResult> PostSalesForecasts(SalesForecastDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _unitOfWork.SalesForecastRepository.CreateAsync(dto);

                if (await _unitOfWork.SaveChangesAsync())
                    return CreatedAtAction("GetSalesForecasts", new { id = dto.Id }, dto);
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message, dto);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesForecasts([FromRoute] int id, [FromBody] SalesForecastDto dto)
        {
            try
            {
                dto.Id = id;
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingForecast = await _unitOfWork.SalesForecastRepository.GetByIdAsync(id);
                if (existingForecast == null)
                    return NotFound($"Could not find Forecast with id {id}");
                await _unitOfWork.SalesForecastRepository.Update(dto);
                if (await _unitOfWork.SaveChangesAsync())
                    return NoContent();
                return BadRequest();

            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }


        [HttpGet("CheckIfExists/{payPeriod}")]
        public async  Task<IActionResult> CheckIfPayPeriodExists(string payPeriod)
        {
            try
            {
                var checkValue =  _unitOfWork.SalesForecastRepository.CheckIfPayPeriodExists(payPeriod);
                if (checkValue)
                    return Ok();
               return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesForecasts(int id)
        {
            try
            {
                var existingForecast = await _unitOfWork.SalesForecastRepository.GetByIdAsync(id);
                if (existingForecast == null)
                    return NotFound($"Could not find Forecast with id {id}");
                _unitOfWork.SalesForecastRepository.Delete(existingForecast);
                if (await _unitOfWork.SaveChangesAsync())
                    return NoContent();
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSalesForecasts()
        {
            var emp = await _unitOfWork.SalesForecastRepository.GetAllAsync();
            return Ok(emp);
        }

     

    }
}
