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

    /// <summary>
    /// Interface for implementation of UserRepository
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Implement specific user query
        /// </summary>
        public Task ExampleQueryAsync();
    }
}
