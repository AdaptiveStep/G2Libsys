namespace G2Libsys.Data.Repository
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region NameSpaces
    using Dapper;
    using G2Libsys.Library.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Transactions;
    #endregion

    // Ändringar här måste även göras i IRepository.
    // Lägg inte till fler saker här, ändra bara
    // befintliga metoder om ej fungerar.

    /// <summary>
    /// Genric repository with method type params
    /// </summary>
    public abstract class GenericRepository : IRepository
    {
        #region Private fields

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
        /// Default constructor, 
        /// tableName = target table in database
        /// </summary>
        public GenericRepository(string tableName = null)
        {
            _tableName = tableName;
            _connectionString = ConfigurationManager.ConnectionStrings["sqldefault"].ConnectionString;
        }

        #endregion

        #region Connection

        protected virtual IDbConnection Connection => new SqlConnection(_connectionString);

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

        public virtual async Task AddRange<T>(IEnumerable<T> items)
        {
            using IDbConnection _db = Connection;

            // Connection open needed for transaction
            _db.Open();

            // Start transaction
            using IDbTransaction transaction = _db.BeginTransaction();
            try
            {
                await _db.ExecuteAsync(
                            sql: GetProcedureName<T>("insertrange"),
                          param: items,
                    commandType: CommandType.StoredProcedure, transaction: transaction);

                // Commit database changes if transaction succeeded
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback databse changes if transaction failed
                transaction.Rollback();
                throw new TransactionAbortedException(ex.ToString());
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

        #region Protected Methods

        /// <summary>
        /// Generate procedure name with name of <typeparamref name="T"/> Model and proc type
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="action">procedure type</param>
        /// <returns></returns>
        protected virtual string GetProcedureName<T>(string action)
        {
            string table = _tableName ?? typeof(T).ToTableName();
            return $"{_prefix}_{action}_{table}";
        }

        #endregion
    }

    /// <summary>
    /// Generic repository where type is predetermined as <typeparamref name="T"/> Model <para/>
    /// NOTE: Use for specific model
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public abstract class GenericRepository<T> : GenericRepository, IRepository<T> 
        where T : class
    {
        #region Constructor

        /// <summary>
        /// Default constructor where tableName = target table in database <para/>
        /// Note: Only specify tablename if needed
        /// </summary>
        public GenericRepository(string tableName = null) 
            : base(tableName) { }

        #endregion

        #region Queries

        public virtual async Task<int> AddAsync(T item) => await base.AddAsync(item);

        public virtual async Task AddRange(IEnumerable<T> items) => await base.AddRange(items);

        public virtual async Task<T> GetByIdAsync(int id) => await base.GetByIdAsync<T>(id);

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await base.GetAllAsync<T>();

        public virtual async Task<IEnumerable<T>> GetRangeAsync(string search) => await base.GetRangeAsync<T>(search);

        public virtual async Task UpdateAsync(T item) => await base.UpdateAsync(item);

        public virtual async Task RemoveAsync(T item) => await base.RemoveAsync(item);

        #endregion
    }

}
