using Nowcfo.Application.DTO;
using Nowcfo.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IMenuRepository
    {
        Task<MenuDto> GetByIdAsync(int id);
        Task<List<MenuDto>> GetAllAsync();
        Task CreateAsync(Menu model);
        void Update(Menu model);
        void Delete(Menu model);
    }
}
