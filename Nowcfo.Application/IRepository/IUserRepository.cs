using Nowcfo.Application.Dtos.User.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IUserRepository
    {
       
        Task<List<UserListDto>> GetAllUsers();

      
    }
}