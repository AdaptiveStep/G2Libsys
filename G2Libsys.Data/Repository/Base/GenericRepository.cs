namespace G2Libsys.Data.Repository
{
    #region NameSpaces
    using Dapper;
    using G2Libsys.Library.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    #endregion

    /// <summary>
    /// Generic repository where <typeparamref name="T"/> is Model
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        #region Privates

        /// <summary>
        /// Default prodecure prefix
        /// </summary>
        private const string _prefix = "usp";

        /// <summary>
        /// Connection string
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Name of target table
        /// </summary>
        private readonly string _tableName;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor where tableName = target table in database
        /// Note: Only specify table name if needed
        /// </summary>
        public GenericRepository(string tableName = null)
        {
            _tableName = tableName ?? typeof(T).ToTableName();
            _connectionString = ConfigurationManager.ConnectionStrings["sqldefault"].ConnectionString;
        }

        #endregion

        #region Connection

        protected IDbConnection Connection => new SqlConnection(_connectionString);

        #endregion

        #region Queries

        public virtual async Task<int> AddAsync(T item)
        {
            // Map item
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(item);
            
            // Create ID parameter for output
            parameters.Add("NewID",
                    dbType: DbType.Int32,
                 direction: ParameterDirection.Output);

            // Open connection
            using IDbConnection _db = Connection;

            // Insert mapped item and set NewID to created item ID
            await _db.ExecuteScalarAsync<int>(
                        sql: GetProcedureName("insert"),
                      param: parameters,
                commandType: CommandType.StoredProcedure);

            // Return the ID of inserted item
            return parameters.Get<int>("NewID");
        }

        public virtual async Task AddRange(IEnumerable<T> item)
        {
            using IDbConnection _db = Connection;

            // Start transaction
            using IDbTransaction transaction = _db.BeginTransaction();

            try
            {
                await _db.ExecuteAsync(
                            sql: GetProcedureName("insertrange"),
                          param: item,
                    commandType: CommandType.StoredProcedure, transaction: transaction);

                // Commit database changes if transaction succeeded
                transaction.Commit();
            }
            catch
            {
                // Rollback databse changes if transaction failed
                transaction.Rollback();
                throw new Exception("Insert Failed");
            }
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            using IDbConnection _db = Connection;

            // Return item with matching id
            return await _db.QueryFirstOrDefaultAsync<T>(
                        sql: GetProcedureName("getbyid"),
                      param: new { id },
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using IDbConnection _db = Connection;

            // Return all items of type T
            return await _db.QueryAsync<T>(
                        sql: GetProcedureName("getall"),
                      param: new { },
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task<IEnumerable<T>> GetRangeAsync(string search)
        {
            using IDbConnection _db = Connection;

            // Return all items matching search
            return await _db.QueryAsync<T>(
                        sql: GetProcedureName("getrange"),
                      param: new { search },
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task UpdateAsync(T item)
        {
            using IDbConnection _db = Connection;

            // Update item in database
            await _db.ExecuteAsync(
                        sql: GetProcedureName("update"),
                      param: item,
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task RemoveAsync(T item)
        {
            using IDbConnection _db = Connection;

            // Remove item from database
            await _db.ExecuteAsync(
                        sql: GetProcedureName("remove"),
                      param: item,
                commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate procedure name with name of <typeparamref name="T"/> Model and proc type
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="action">procedure type</param>
        /// <returns></returns>
        private string GetProcedureName(string action)
        {
            return $"{_prefix}_{action}_{_tableName}";
        }

        #endregion
    }

    /// <summary>
    /// Repository with method type params
    /// </summary>
    public abstract class GenericRepository : IRepository
    {
        #region Privates

        /// <summary>
        /// Default prodecure prefix
        /// </summary>
        private const string _prefix = "usp";

        /// <summary>
        /// Connection string
        /// </summary>
        private readonly string _connectionString;

        #endregion

        #region Constructor

        /// <summary>
        /// tableName = target table in database
        /// </summary>
        public GenericRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["sqldefault"].ConnectionString;
        }

        #endregion

        #region Connection

        protected IDbConnection Connection => new SqlConnection(_connectionString);

        #endregion

        #region Queries

        public virtual async Task<int> AddAsync<T>(T item)
        {
            // Map item
            DynamicParameters parameters = new DynamicParameters();
            parameters.AddDynamicParams(item);

            // Create ID parameter for output
            parameters.Add("NewID",
                    dbType: DbType.Int32,
                 direction: ParameterDirection.Output);

            // Open connection
            using IDbConnection _db = Connection;

            // Insert mapped item and set NewID to created item ID
            await _db.ExecuteScalarAsync<int>(
                        sql: GetProcedureName<T>("insert"), 
                      param: parameters, 
                commandType: CommandType.StoredProcedure);

            // Return the ID of inserted item
            return parameters.Get<int>("NewID");
        }

        public virtual async Task AddRange<T>(IEnumerable<T> item)
        {
            using IDbConnection _db = Connection;

            // Start transaction
            using IDbTransaction transaction = _db.BeginTransaction();

            try
            {
                await _db.ExecuteAsync(
                            sql: GetProcedureName<T>("insertrange"), 
                          param: item, 
                    commandType: CommandType.StoredProcedure, transaction: transaction);

                // Commit database changes if transaction succeeded
                transaction.Commit();
            }
            catch
            {
                // Rollback databse changes if transaction failed
                transaction.Rollback();
                throw new Exception("Insert Failed");
            }
        }

        public virtual async Task<T> GetByIdAsync<T>(int id)
        {
            using IDbConnection _db = Connection;

            // Return item with matching id
            return await _db.QueryFirstOrDefaultAsync<T>(
                        sql: GetProcedureName<T>("getbyid"), 
                      param: new { id }, 
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            using IDbConnection _db = Connection;

            // Return all items of type T
            return await _db.QueryAsync<T>(
                        sql: GetProcedureName<T>("getall"), 
                      param: new { }, 
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task<IEnumerable<T>> GetRangeAsync<T>(string search)
        {
            using IDbConnection _db = Connection;

            // Return all items matching search
            return await _db.QueryAsync<T>(
                        sql: GetProcedureName<T>("getrange"), 
                      param: new { search }, 
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task UpdateAsync<T>(T item)
        {
            using IDbConnection _db = Connection;

            // Update item in database
            await _db.ExecuteAsync(
                        sql: GetProcedureName<T>("update"), 
                      param: item, 
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task RemoveAsync<T>(T item)
        {
            using IDbConnection _db = Connection;

            // Remove item from database
            await _db.ExecuteAsync(
                        sql: GetProcedureName<T>("remove"), 
                      param: item, 
                commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generate procedure name with name of <typeparamref name="T"/> Model and proc type
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="action">procedure type</param>
        /// <returns></returns>
        private string GetProcedureName<T>(string action)
        {
            return $"{_prefix}_{action}_{typeof(T).ToTableName()}";
        }

        #endregion
    }
}
