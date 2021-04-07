using AutoMapper;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.IRepository;

namespace Nowcfo.API.Controllers
{
    public class RolePermissionController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RolePermissionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
