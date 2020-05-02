namespace G2Libsys.Data.Repository
{
    using Dapper;

    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using G2Libsys.Library;
    using System;
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
        public UserRepository() 
            : base() { }

        /// <summary>
        /// Example User specific query
        /// </summary>
        public async Task<User> VerifyLoginAsync(string email, string password)
        {
            using IDbConnection _db = base.Connection;

            // Fetch user with correct username and password
            return await _db.QueryFirstAsync<User>(
                        sql: GetProcedureName<User>("verifylogin"),
                      param: new { email, password },
                commandType: CommandType.StoredProcedure);
        }
    }
}
