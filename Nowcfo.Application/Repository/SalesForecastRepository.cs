using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.Helper.Pagination;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.Application.Repository
{
    public class SalesForecastRepository : ISalesForecastRepository

    {

        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public SalesForecastRepository(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }


        public async Task<PagedList<SalesForecastDto>> GetPagedListAsync(Param param)
        {
            try
            {

                var result = ( _dbContext.SalesForecasts.Select(t => new SalesForecastDto()
                {

                    Id = t.Id,
                    PayPeriod = t.PayPeriod.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    BillRateCurrency = string.Format("{0:c0}", t.BillRate),
                    BillRate = t.BillRate,
                    BillHours = t.BillHours,
                    Placements = t.Placements,
                    BuyOuts = t.BuyOuts,
                    EstimatedRevenue = t.EstimatedRevenue,
                    Cogs = t.Cogs,
                    CogsQkly = t.CogsQkly,
                    ClosedPayPeriods = t.ClosedPayPeriods,
                    OtherPercent = t.OtherPercent

                }));
            return await PagedList<SalesForecastDto>.CreateAsync(result, param.PageNumber, param.PageSize);


            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task<SalesForecastDto> GetByIdAsync(int id)
        {
            try
            {

                return await _dbContext.SalesForecasts.AsNoTracking().Select(t => new SalesForecastDto()
                {
                    Id = t.Id,
                    PayPeriod=t.PayPeriod.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    BillRate=t.BillRate,
                    BillHours=t.BillHours,
                    Placements=t.Placements,
                    BuyOuts=t.BuyOuts,
                    EstimatedRevenue=t.EstimatedRevenue,
                    Cogs=t.Cogs,
                    CogsQkly=t.CogsQkly,
                    ClosedPayPeriods=t.ClosedPayPeriods,
                    OtherPercent=t.OtherPercent

    }).Where(m=>m.Id==id).FirstOrDefaultAsync();


            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        public async Task<List<SalesForecastDto>> GetAllAsync()
        {
            try
            {

                return  await _dbContext.SalesForecasts.Select(t => new SalesForecastDto() {

                    Id = t.Id,
                    PayPeriod = t.PayPeriod.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    BillRateCurrency = string.Format("{0:c0}", t.BillRate),
                    BillRate= t.BillRate,
                    BillHours = t.BillHours,
                    Placements = t.Placements,
                    BuyOuts = t.BuyOuts,
                    EstimatedRevenue = t.EstimatedRevenue,
                    Cogs = t.Cogs,
                    CogsQkly = t.CogsQkly,
                    ClosedPayPeriods = t.ClosedPayPeriods,
                    OtherPercent = t.OtherPercent

                }).ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task CreateAsync(SalesForecastDto model)
        {
            try
            {

                var forecast = _mapper.Map<SalesForecastDto, SalesForecast>(model);
                await _dbContext.SalesForecasts.AddAsync(forecast);

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task Update(SalesForecastDto model)
        {
            try
            {
                var existingForecast = await GetByIdAsync(model.Id);
                var forecast = _mapper.Map<SalesForecastDto, SalesForecast>(model);
                _dbContext.SalesForecasts.Update(forecast);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        

        public bool CheckIfPayPeriodExists(string payPeriod)
        {

            var ifExists = _dbContext.SalesForecasts.Any(x => x.PayPeriod == Convert.ToDateTime(payPeriod));
            return ifExists;
        }

        public void Delete(SalesForecastDto model)
        {
            try
            {
                var forecast = _mapper.Map<SalesForecastDto, SalesForecast>(model);
                _dbContext.SalesForecasts.Remove(forecast);
            }

            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }


       
    }
}
