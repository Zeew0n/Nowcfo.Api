using Dapper;
using Microsoft.Extensions.Configuration;
using Nowcfo.Application.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.Application.Repository
{
    public class DapperRepository : IDapperRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionstring;
        private readonly int _timeOut;

        public DapperRepository(IConfiguration config)
        {
            _config = config;
            _connectionstring = _config.GetConnectionString("DefaultConnection");
            _timeOut = 300;
        }

        public DapperRepository(string connectionString)
        {
            _connectionstring = connectionString;
        }

        public T Get<T>(string sp, DynamicParameters parms = null)
        {
            using IDbConnection db = new SqlConnection(_connectionstring);
            return db.Query<T>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut).FirstOrDefault();
        }

        public async Task<int> CountAsync(string sp, DynamicParameters parms = null)
        {
            using IDbConnection db = new SqlConnection(_connectionstring);
            return await db.ExecuteScalarAsync<int>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut);
        }

        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms = null)
        {
            using IDbConnection db = new SqlConnection(_connectionstring);
            return (await db.QueryAsync<T>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut)).FirstOrDefault();
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms = null)
        {
            using IDbConnection db = new SqlConnection(_connectionstring);
            return db.Query<T>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut).ToList();
        }

        public async Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms = null)
        {
            using IDbConnection db = new SqlConnection(_connectionstring);
            return (await db.QueryAsync<T>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut)).ToList();
        }

        public (object, List<T>) GetAll<T>(string sp, DynamicParameters parms, string outputParameter)
        {
            using IDbConnection db = new SqlConnection(_connectionstring);
            var result = db.Query<T>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut).ToList();
            var output = parms.Get<object>(outputParameter);
            return (output, result);
        }

        public async Task<(object, List<T>)> GetAllAsync<T>(string sp, DynamicParameters parms, string outputParameter)
        {
            using IDbConnection db = new SqlConnection(_connectionstring);
            var result = (await db.QueryAsync<T>(sp, parms, commandType: CommandType.StoredProcedure, commandTimeout: _timeOut)).ToList();
            var output = parms.Get<object>(outputParameter);
            return (output, result);
        }

        public int Insert(string sp, DynamicParameters parms = null)
        {
            int result;
            using IDbConnection db = new SqlConnection(_connectionstring);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Execute(sp, parms, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: _timeOut);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public (object, int) Insert(string sp, DynamicParameters parms, string outputParameter)
        {
            int result;
            object output;
            using IDbConnection db = new SqlConnection(_connectionstring);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Execute(sp, parms, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: _timeOut);
                    tran.Commit();
                    output = parms.Get<object>(outputParameter);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return (output, result);
        }

        public async Task<int> InsertAsync(string sp, DynamicParameters parms = null)
        {
            int result;
            using IDbConnection db = new SqlConnection(_connectionstring);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = await db.ExecuteAsync(sp, parms, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: _timeOut);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public async Task<(object, int)> InsertAsync(string sp, DynamicParameters parms, string outputParameter)
        {
            int result;
            object output;
            using IDbConnection db = new SqlConnection(_connectionstring);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = await db.ExecuteAsync(sp, parms, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: _timeOut);
                    tran.Commit();
                    output = parms.Get<object>(outputParameter);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return (output, result);
        }

        public int Update(string sp, DynamicParameters parms = null)
        {
            int result;
            using IDbConnection db = new SqlConnection(_connectionstring);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Execute(sp, parms, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: _timeOut);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public async Task<int> UpdateAsync(string sp, DynamicParameters parms = null)
        {
            int result;
            using IDbConnection db = new SqlConnection(_connectionstring);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = await db.ExecuteAsync(sp, parms, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: _timeOut);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
    }

}
