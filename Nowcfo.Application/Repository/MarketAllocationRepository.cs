using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.Application.Repository
{
    public class MarketAllocationRepository : IMarketAllocationRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public MarketAllocationRepository(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }


        public async Task<MarketMasterDto> GetByIdAsync(int id)
        {
            try
            {
                return await (from o in _dbContext.MarketMasters
                              
                              where o.Id == id
                              select new MarketMasterDto
                              {
                                  Id = o.Id,
                                  AllocationTypeId = o.AllocationTypeId,
                                  PayPeriod = o.PayPeriod,
                                  OrganizationId = o.OrganizationId,

                              }).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }


        public async Task CreateAsync(MarketMasterDto model)
        {
            try
            {
                var market = _mapper.Map<MarketMasterDto, MarketMaster>(model);
                await _dbContext.MarketMasters.AddAsync(market);
                var m= _dbContext.SaveChange();
                var n = market.Id;

                foreach(var  marketAllocation  in model.MarketAllocationDto)
                {
                    var marketModel = new MarketAllocation
                    {
                        MasterId = n,
                        MarketId=marketAllocation.MarketId,
                        Revenue=marketAllocation.Revenue,
                        COGS=marketAllocation.COGS,
                        CogsTypeId=marketAllocation.CogsTypeId,
                        SE=marketAllocation.SE,
                        EE= marketAllocation.EE,
                        GA=marketAllocation.GA,
                        Other=marketAllocation.Other,
                        OtherTypeId=marketAllocation.OtherTypeId
                    };
                    await _dbContext.MarketAllocations.AddAsync(marketModel);
                    _dbContext.SaveChange();

                }
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }

        }



        public async Task<List<OrganizationDto>> GetAllOrganizations()
        {
            try
            {
                var organizations = await _dbContext.Organizations.Where(m => m.ParentOrganizationId == null).ToListAsync();
                return _mapper.Map<List<OrganizationDto>>(organizations);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }



        public async Task<List<MarketMasterDto>> GetAllMarketList(int id)
        {
            try
            {

                var marketList = await (from op in _dbContext.MarketMasters.Where(x => x.Id == id)

                                        select new MarketMasterDto

                                        {
                                            Id = op.Id,
                                            AllocationTypeId = op.AllocationTypeId,
                                            PayPeriod = op.PayPeriod
                                        }).ToListAsync();

                return _mapper.Map<List<MarketMasterDto>>(marketList);

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }
        //GetAllMarketsByOrgId

        public async Task<List<MarketAllocationDto>> GetAllMarketsByOrgId(int orgId)
        {
            try
            {

                var allocationList = await (from os in _dbContext.Organizations.Where(x => x.ParentOrganizationId == orgId)
                                            select new MarketAllocationDto
                                            {
                                                //newly created in this case
                                                //MasterId = 0,
                                                MarketId =os.OrganizationId,
                                                Revenue = 0,
                                                COGS = 0,
                                                CogsTypeId = 1,
                                                SE = 0,
                                                EE = 0,
                                                GA = 0,
                                                Other = 0,
                                                OtherTypeId = 1
                                            }).ToListAsync();

                return _mapper.Map<List<MarketAllocationDto>>(allocationList);

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }




        public async Task<List<MarketAllocationDto>> GetAllAllocationsById(int masterId, string payPeriod, int id, int allocationTypeId)
        {
            try
            {

                var allocationList = await (from op in _dbContext.MarketMasters.Where(x => x.Id == id).Where(x => x.OrganizationId == masterId).Where(x => x.PayPeriod.ToString() == payPeriod).Where(x => x.AllocationTypeId == allocationTypeId)
                                            join os in _dbContext.Organizations on op.OrganizationId equals os.OrganizationId
                                            join ma in _dbContext.MarketAllocations on op.Id equals ma.MarketId

                                               select new MarketAllocationDto

                                               {
                                                   MasterId = op.Id,
                                                   MarketId = op.OrganizationId,
                                                   Revenue = 0,
                                                   COGS = 0,
                                                   CogsTypeId = 1,
                                                   SE = 0,
                                                   EE = 0,
                                                   GA = 0,
                                                   Other = 0,
                                                   OtherTypeId = 1
                                               }).ToListAsync();

                return _mapper.Map<List<MarketAllocationDto>>(allocationList);

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }



        public void Delete(MarketMasterDto model)
        {
            try
            {
                var master = _mapper.Map<MarketMasterDto, MarketMaster>(model);
                _dbContext.MarketMasters.Remove(master);
            }

            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task Update(MarketMasterDto model)
        {
            try
            {
                var marketAlloc = new List<MarketAllocation>();
                var master = _mapper.Map<MarketMasterDto,MarketMaster>(model);

                _dbContext.MarketMasters.Update(master);
                var y = _dbContext.SaveChange();
                var x = master.Id;


                var allocations = _dbContext.MarketAllocations.Where(z => z.MasterId == x).ToListAsync();
                _dbContext.MarketAllocations.RemoveRange(allocations.Result);
                _dbContext.SaveChange();


                foreach (var marketAllocation in model.MarketAllocationDto)
                {
                    var marketModel = new MarketAllocation
                    {
                        MasterId = x,
                        MarketId = marketAllocation.MarketId,
                        Revenue = marketAllocation.Revenue,
                        COGS = marketAllocation.COGS,
                        CogsTypeId = marketAllocation.CogsTypeId,
                        SE = marketAllocation.SE,
                        EE = marketAllocation.EE,
                        GA = marketAllocation.GA,
                        Other = marketAllocation.Other,
                        OtherTypeId = marketAllocation.OtherTypeId
                    };
                    marketAlloc.Add(marketModel);
                }
                _dbContext.MarketAllocations.UpdateRange(marketAlloc);
                _dbContext.SaveChange();

            }
            catch(Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;

            }
        }
    }
}
