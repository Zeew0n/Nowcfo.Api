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
    public class MenuRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public MenuRepository(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<MenuDto> GetByIdAsync(string id)
        {
            try
            {
                var menu = await _dbContext.Menus.AsNoTracking().SingleOrDefaultAsync(x => x.MenuId == id);
                return _mapper.Map<MenuDto>(menu);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public async Task<List<MenuDto>> GetAllAsync()
        {
            try
            {
                var menus = await _dbContext.Menus .ToListAsync();
                return _mapper.Map<List<MenuDto>>(menus);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public async Task CreateAsync(Menu model)
        {
            try
            {
                await _dbContext.Menus.AddAsync(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Update(Menu model)
        {
            try
            {
                _dbContext.Menus.Update(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Delete(Menu model)
        {
            try
            {
                _dbContext.Menus.Remove(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
