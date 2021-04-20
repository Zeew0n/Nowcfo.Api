using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Belgrade.SqlClient;
using Microsoft.Data.SqlClient;
using Nowcfo.API.Interface;
using Serilog;

namespace Nowcfo.API.Implementation
{
    public class SqlRestService : ISqlRestService
    {
        private readonly IQueryPipe _query;

        public SqlRestService(IQueryPipe query) => _query = query;

        public async Task ExecuteQueryStoredProcedure(string procedureName, Stream responseBody, Dictionary<string, object> parameters)
        {
            try
            {               
                using (SqlCommand sqlCommand = new SqlCommand(procedureName))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }
                    await _query.Sql(sqlCommand).Stream(responseBody, "{}");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error: {ErrorMessage},{ErrorDetails}", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}