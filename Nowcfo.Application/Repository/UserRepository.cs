using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nowcfo.Application.IRepository;
using Nowcfo.Domain.Models.AppUserModels;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Nowcfo.Application.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(IApplicationDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<AppUser> GetUserById(Guid userId)
        {
            try
            {
                var user = await _dbContext.AppUsers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
                return _mapper.Map<AppUser>(user);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }

        public void Update(AppUser model)
        {
            try
            {
                _dbContext.AppUsers.Update(model);
            }
            catch (Exception e)
            {
                Log.Error("Error: { ErrorMessage},{ ErrorDetails}", e.Message, e.StackTrace);
                throw;
            }
        }
    }
}
