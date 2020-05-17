namespace G2Libsys.Data.Repository
{
    using Dapper;

    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using G2Libsys.Library;
    using System.Data;
    using System.Threading.Tasks;
    #endregion

    // Ändringar här måste även göras i IUserRepository

    /// <summary>
    /// User repository for implementation of specific queries
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UserRepository() { }

        public override async Task<int> AddAsync(User item)
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
                        sql: GetProcedureName<User>("insert"),
                      param: parameters,
                commandType: CommandType.StoredProcedure);

            // Return the ID of inserted item
            return parameters.Get<int>("NewID");
        }

        /// <summary>
        /// Example User specific query
        /// </summary>
        public async Task<User> VerifyLoginAsync(string email, string password)
        {
            using IDbConnection _db = base.Connection;

            // Fetch user with correct username and password
            return await _db.QueryFirstOrDefaultAsync<User>(
                        sql: GetProcedureName<User>("verifylogin"),
                      param: new { email, password },
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Check if email already exist
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public async Task<bool> VerifyEmailAsync(string email)
        {
            using IDbConnection _db = base.Connection;

            // Return true if email exist in db
            return await _db.ExecuteScalarAsync<bool>(
                        sql: GetProcedureName<User>("verifyemail"),
                      param: new { email },
                commandType: CommandType.StoredProcedure);
        }
    }
}
