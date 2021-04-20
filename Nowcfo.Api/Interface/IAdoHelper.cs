using System.Collections.Generic;

namespace Nowcfo.API.Interface
{
    public interface IAdoHelper<T> where T : class
    {
        IReadOnlyCollection<T> ExecuteRawSql(string rawSql);

        IReadOnlyCollection<T> ExecuteRawSql(string rawSql, Dictionary<string, object> parameters);

        IReadOnlyCollection<T> ExecuteStoredProcedure(string procedureName);

        IReadOnlyCollection<T> ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters);
    }
}