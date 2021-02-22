using Nowcfo.Application.Dtos.User.Response;
using Nowcfo.Domain.Models.AppUserModels;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IUserRepository
    {
       
        Task<AppUser> GetUserById(System.Guid userId);
        void Update(AppUser model);
    }   
}