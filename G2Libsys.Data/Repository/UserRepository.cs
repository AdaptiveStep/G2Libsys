namespace G2Libsys.Data.Repository
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using G2Libsys.Library;
    using System;
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
        public UserRepository() : base()
        {

        }

        /// <summary>
        /// Example User specific query
        /// </summary>
        public Task ExampleQueryAsync()
        {
            using var _db = base.Connection;
            var procedureName = base.GetProcedureName<User>("ProcedureActionName");
            _db.Close();
            throw new NotImplementedException();
        }
    }
}
