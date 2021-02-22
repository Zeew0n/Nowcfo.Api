using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.DTO;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.Repository
{
    public class OrganizationRepository:IOrganizationRepository 
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrganizationRepository(IApplicationDbContext context,IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<OrganizationDto> GetByIdAsync(int id)
        {
            try
            {
               
                var organization = await _dbContext.Organizations.AsNoTracking().SingleOrDefaultAsync(x=>x.OrganizationId==id);
                return _mapper.Map<OrganizationDto>(organization);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<List<OrganizationDto>> GetAllAsync()
        {
            try
            {
                var organizations =  await _dbContext.Organizations.ToListAsync();
                return _mapper.Map<List<OrganizationDto>>(organizations);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task CreateAsync(Organization model)
        {
            try
            {
                await _dbContext.Organizations.AddAsync(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Update(Organization model)
        {
            try
            {
                _dbContext.Organizations.Update(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Delete(Organization model)
        {
            try
            {
                _dbContext.Organizations.Remove(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
