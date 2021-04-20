using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nowcfo.API.Interface;
using Nowcfo.Infrastructure.Data;

namespace Nowcfo.API.Implementation
{
    public class AdoHelper<T> : IAdoHelper<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;

        public AdoHelper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = InitializeConnectionString();
        }

        public IReadOnlyCollection<T> ExecuteRawSql(string rawSql)
        {
            DataTable dtData = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(rawSql, sqlConn))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(dtData);
                    }
                }
            }
            return ConvertToList(dtData);
        }

        public IReadOnlyCollection<T> ExecuteRawSql(string rawSql, Dictionary<string, object> parameters)
        {
            DataTable dtData = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(rawSql, sqlConn))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        var parameter = GenerateParameters(parameters);
                        sqlCommand.Parameters.AddRange(parameter);
                    }
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(dtData);
                    }
                }
            }
            return ConvertToList(dtData);
        }

        private SqlParameter[] GenerateParameters(Dictionary<string, object> parameters)
        {
            return _().ToArray(); IEnumerable<SqlParameter> _()
            {
                foreach (var item in parameters)
                {
                    yield return new SqlParameter(item.Key, item.Value ?? DBNull.Value);
                }
            }
        }

        public IReadOnlyCollection<T> ExecuteStoredProcedure(string procedureName)
        {
            DataTable dtData = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(dtData);
                    }
                }
            }
            return ConvertToList(dtData);
        }

        public IReadOnlyCollection<T> ExecuteStoredProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            DataTable dtData = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConn))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        var parameter = GenerateParameters(parameters);
                        sqlCommand.Parameters.AddRange(parameter);
                    }
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(dtData);
                    }
                }
            }
            return ConvertToList(dtData);
        }

        private string InitializeConnectionString() => _dbContext.Database.GetDbConnection().ConnectionString;

        private static List<T> ConvertToList(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                                .Select(c => c.ColumnName)
                                .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        object dtRow = row[pro.Name];
                        Type propertyType = pI.PropertyType;
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(dtRow, propertyType));
                    }
                }
                return objT;
            }).ToList();
        }
    }
}