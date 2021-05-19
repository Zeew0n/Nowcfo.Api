using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.Helper.Pagination;
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
        private readonly IDapperRepository _dapper;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public MarketAllocationRepository(IApplicationDbContext context, IMapper mapper,IDapperRepository dapper)
        {
            _dbContext = context;
            _mapper = mapper;
            _dapper = dapper;
        }


        public async Task<PagedList<MarketMasterDto>> GetPagedListAsync(ParamMarket param)
        {
            try
            {

                var marketList = (from op in _dbContext.MarketMasters.Where(x => x.OrganizationId==param.SearchOrg)
                                        join at in _dbContext.AllocationTypes on op.AllocationTypeId equals at.Id
                                        select new MarketMasterDto

                                        {
                                            Id = op.Id,
                                            AllocationTypeId = op.AllocationTypeId,
                                            AllocationName = at.Name,
                                            PayPeriod = op.PayPeriod,
                                        });

                return await PagedList<MarketMasterDto>.CreateAsync(marketList, param.PageNumber, param.PageSize);

                       
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task<MarketMasterDto> GetByIdAsync(int id)
        {
            try
            {
                return await (from o in _dbContext.MarketMasters
                              join at in _dbContext.AllocationTypes on o.AllocationTypeId equals at.Id
                              where o.Id == id
                              select new MarketMasterDto
                              {
                                  Id = o.Id,
                                  AllocationTypeId = o.AllocationTypeId,
                                  AllocationName=at.Name,
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
                 _dbContext.MarketMasters.Add(market);
                _dbContext.SaveChange();
                foreach(var  marketAllocation  in model.MarketAllocations)
                {
                    var marketModel = new MarketAllocation
                    {
                        MasterId = market.Id,
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
                var marketList = await (from m in _dbContext.MarketMasters.Include(x=>x.MarketAllocations).Where(x => x.OrganizationId == id)
                    join o in _dbContext.Organizations on m.OrganizationId equals o.OrganizationId
                    select new MarketMasterDto

                    {
                        OrganizationId=id,
                        OrganizationName = o.OrganizationName,
                        PayPeriod = m.PayPeriod,
                        AllocationTypeId = m.AllocationTypeId,
                        MarketAllocations = _mapper.Map<List<MarketAllocationDto>>(m.MarketAllocations)
                    }).ToListAsync();

                return marketList;


                //var marketList = await (from op in _dbContext.MarketMasters.Where(x => x.OrganizationId == id)
                //                        join at in _dbContext.AllocationTypes on op.AllocationTypeId equals at.Id
                //                        select new MarketMasterDto

                //                        {
                //                            Id = op.Id,
                //                            AllocationTypeId = op.AllocationTypeId,
                //                            AllocationName = at.Name,
                //                            PayPeriod = op.PayPeriod.ToShortDateString(),
                //                        }).ToListAsync();

                //return _mapper.Map<List<MarketMasterDto>>(marketList);

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task<List<MarketDto>> GetAllMarketsByOrgIdXXX(int orgId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@OrgId", orgId);
                var result = await _dapper.GetAllAsync<MarketDto>("GetAllMarkets", param);
                return result;

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task<List<MarketAllocationDto>> GetAllMarketsByOrgId(int orgId)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@OrgId", orgId);
                var result = await _dapper.GetAllAsync<MarketAllocationDto>("GetAllMarkets", param);
                return result;

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
                DynamicParameters param = new DynamicParameters();
                param.Add("@OrgId", masterId);
                param.Add("@payperiod", payPeriod);
                param.Add("@allocationtypeid", allocationTypeId);
                param.Add("@id", id);

                var result = await _dapper.GetAllAsync<MarketAllocationDto>("GetAllMarketsByParamas", param);
                return result;

            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }



        public async Task<List<AllocationTypeDto>> GetAllocationTypes()
        {
            try
            {
                var allocations = await _dbContext.AllocationTypes.ToListAsync();
                return _mapper.Map<List<AllocationTypeDto>>(allocations);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task<List<CogsTypeDto>> GetCogsTypes()
        {
            try
            {
                var cogs = await _dbContext.CogsTypes.ToListAsync();
                return _mapper.Map<List<CogsTypeDto>>(cogs);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }


        public async Task<List<OtherTypeDto>> GetOtherTypes()
        {
            try
            {
                var others = await _dbContext.OtherTypes.ToListAsync();
                return _mapper.Map<List<OtherTypeDto>>(others);
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


                foreach (var marketAllocation in model.MarketAllocations)
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
