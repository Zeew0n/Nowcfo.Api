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
    public class DesignationRepository : IDesignationRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public DesignationRepository(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<DesignationDto> GetByIdAsync(int id)
        {
            try
            {

                var employee = await _dbContext.Designations.AsNoTracking().SingleOrDefaultAsync(x => x.DesignationId == id);
                return _mapper.Map<DesignationDto>(employee);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<List<DesignationDto>> GetAllAsync()
        {
            try
            {
                var designations = await _dbContext.Designations.ToListAsync();
                return _mapper.Map<List<DesignationDto>>(designations);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task CreateAsync(Designation model)
        {
            try
            {
                await _dbContext.Designations.AddAsync(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Update(Designation model)
        {
            try
            {
                _dbContext.Designations.Update(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Delete(Designation model)
        {
            try
            {
                _dbContext.Designations.Remove(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
