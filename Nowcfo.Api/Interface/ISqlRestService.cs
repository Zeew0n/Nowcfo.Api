using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Nowcfo.API.Interface
{
    public interface ISqlRestService
    {
        Task ExecuteQueryStoredProcedure(string procedureName, Stream responseBody, Dictionary<string, object> parameters);
    }
}