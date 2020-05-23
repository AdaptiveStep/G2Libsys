namespace G2Libsys.Data.Repository
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region NameSpaces
    using Dapper;
	using G2Libsys.Library;
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
        private string bkupString;

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
            _connectionString = ConfigurationManager.ConnectionStrings["sqlexpress"].ConnectionString;
        }

        #endregion

        #region Connection

        //protected virtual IDbConnection GetConnection => new SqlConnection(_connectionString);

        // Temporär lösning för flera connectionstrings
        protected virtual IDbConnection Connection
        {
            get
            {
                try
                {
                    var conn = new SqlConnection(_connectionString);
                    conn.Open();
                    return conn;
                }
                catch
                {
                    try
                    {
                        bkupString = ConfigurationManager.ConnectionStrings["sqldefault"].ConnectionString;
                        var conn = new SqlConnection(bkupString);
                        conn.Open();
                        return conn;
                    }
                    catch
                    {
                        bkupString = ConfigurationManager.ConnectionStrings["sqlcustom"].ConnectionString;
                        var conn = new SqlConnection(bkupString);
                        conn.Open();
                        return conn;
                    }
                }
            }
        }

        #endregion

        #region Queries

        public virtual async Task<int> AddAsync<T>(T item)
        {
            // Open connection
            using IDbConnection _db = Connection;

            // Insert item and return affected rows
            return await _db.ExecuteAsync(
                        sql: GetProcedureName<T>("insert"),
                      param: item,
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task AddRangeAsync<T>(IEnumerable<T> items)
        {
            using IDbConnection _db = Connection;

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

        public virtual async Task<IEnumerable<T>> GetAllAsync<T>(int? id = null)
        {
            var param = id == null ? (object)(new { }) : new { id };

            using IDbConnection _db = Connection;

            // Return all items of type T
            return await _db.QueryAsync<T>(
                        sql: GetProcedureName<T>("getall"),
                      param: param,
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task<IEnumerable<T>> GetRangeAsync<T>(string search)
        {
            using IDbConnection _db = Connection;

            // Return all items matching search
            return await _db.QueryAsync<T>(
                        sql: GetProcedureName<T>("simplesearch"),
                      param: new { search },
                commandType: CommandType.StoredProcedure);
        }

        public virtual async Task<IEnumerable<T>> GetRangeAsync<T>(T item)
        {
            using IDbConnection _db = Connection;

            // Return all items matching search with multiple filters
            return await _db.QueryAsync<T>(
                        sql: GetProcedureName<T>("filtersearch"),
                      param: item,
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

        public virtual async Task RemoveAsync<T>(int id)
        {
            using IDbConnection _db = Connection;

            // Remove item from database
            await _db.ExecuteAsync(
                        sql: GetProcedureName<T>("remove"),
                      param: new { id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<LibraryObject>> AdvancedSearchAsync(LibraryObject paramsInObject)
        {
            using (IDbConnection _db = Connection)
            {

                // Return all items of type T
                var tmp = await _db.QueryAsync<LibraryObject>(
                           sql: GetProcedureName<User>("smart_filter_Search"),
                         param: paramsInObject,
                   commandType: CommandType.StoredProcedure);

                return tmp;
            }
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

        public virtual async Task AddRangeAsync(IEnumerable<T> items) => await base.AddRangeAsync(items);

        public virtual async Task<T> GetByIdAsync(int id) => await base.GetByIdAsync<T>(id);

        public virtual async Task<IEnumerable<T>> GetAllAsync(int? id = null) => await base.GetAllAsync<T>(id);

        public virtual async Task<IEnumerable<T>> GetRangeAsync(string partialword) => await base.GetRangeAsync<T>(partialword);

        public virtual async Task<IEnumerable<T>> GetRangeAsync(T item) => await base.GetRangeAsync(item);

        public virtual async Task UpdateAsync(T item) => await base.UpdateAsync(item);

        public virtual async Task RemoveAsync(int id) => await base.RemoveAsync<T>(id);


    }
    #endregion
}
