using Nowcfo.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IMenuRepository
    {
        Task<List<MenuDto>> GetMenusByUserRoleAsync(Guid userId);
    }
}
