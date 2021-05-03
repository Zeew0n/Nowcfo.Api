﻿using AutoMapper;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models;
using Serilog;
using System;
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

        public void Delete(MarketMasterDto model)
        {
            throw new System.NotImplementedException();
        }


        public void Update(MarketMasterDto model)
        {
            throw new System.NotImplementedException();
        }
    }
}