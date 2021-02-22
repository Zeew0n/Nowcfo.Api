using Nowcfo.Application.IRepository;

namespace Nowcfo.Application.Repository
{
    public class UserRepository
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _dbContext = context;
            _unitOfWork = unitOfWork;
        }

        
    }
}
