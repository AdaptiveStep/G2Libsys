using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;

namespace G2Libsys.Data.Repository
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly string _tableName;
        private readonly string _connectionString;

        /// <summary>
        /// tableName = target table in database
        /// </summary>
        public GenericRepository(string tableName)
        {
            _tableName = tableName;
            _connectionString = ConfigurationManager.ConnectionStrings["sqldefault"].ConnectionString;
        }

        protected IDbConnection Connection => new SqlConnection(_connectionString);

        // Create
        public async Task<int> AddAsync(T item)
        {
            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(item);
            parameters.Add("NewID",
                    dbType: DbType.Int32,
                 direction: ParameterDirection.Output);

            using var _db = Connection;
            await _db.ExecuteScalarAsync<int>("usp_insert_" + _tableName, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("NewID");
        }

        public async Task AddRange(IEnumerable<T> item)
        {
            using var _db = Connection;
            using var transaction = _db.BeginTransaction();
            try
            {
                await _db.ExecuteAsync("usp_insertrange_" + _tableName, item, commandType: CommandType.StoredProcedure, transaction: transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw new Exception("Insert Failed");
            }
        }

        // Read
        public async Task<T> GetByIdAsync(int id)
        {
            using var _db = Connection;
            return await _db.QueryFirstAsync<T>("usp_getbyid_" + _tableName, new { id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var _db = Connection;
            return await _db.QueryAsync<T>("usp_getall_" + _tableName, new { }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<T>> GetRangeAsync(string search)
        {
            using var _db = Connection;
            return await _db.QueryAsync<T>("usp_getrange_" + _tableName, new { search }, commandType: CommandType.StoredProcedure);
        }

        // Update
        public async Task UpdateAsync(T item)
        {
            using var _db = Connection;
            await _db.ExecuteAsync("usp_update_" + _tableName, item, commandType: CommandType.StoredProcedure);
        }

        // Delete
        public async Task RemoveAsync(T item)
        {
            using var _db = Connection;
            await _db.ExecuteAsync("usp_remove_" + _tableName, item, commandType: CommandType.StoredProcedure);
        }
    }
}
