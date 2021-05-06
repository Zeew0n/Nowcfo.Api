using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nowcfo.Application.IRepository
{
    public interface IDapperRepository
    {
        T Get<T>(string sp, DynamicParameters parms = null);

        Task<int> CountAsync(string sp, DynamicParameters parms = null);

        Task<T> GetAsync<T>(string sp, DynamicParameters parms = null);

        List<T> GetAll<T>(string sp, DynamicParameters parms = null);

        Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms = null);

        (object, List<T>) GetAll<T>(string sp, DynamicParameters parms, string outputParameter);

        Task<(object, List<T>)> GetAllAsync<T>(string sp, DynamicParameters parms, string outputParameter);

        int Insert(string sp, DynamicParameters parms = null);

        (object, int) Insert(string sp, DynamicParameters parms, string outputParameter);

        Task<int> InsertAsync(string sp, DynamicParameters parms = null);

        Task<(object, int)> InsertAsync(string sp, DynamicParameters parms, string outputParameter);

        int Update(string sp, DynamicParameters parms = null);

        Task<int> UpdateAsync(string sp, DynamicParameters parms = null);
    }
}
