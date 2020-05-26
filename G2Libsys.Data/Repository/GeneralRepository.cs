namespace G2Libsys.Data.Repository
{
    /// <summary>
    /// Implements a repository that can be used with any model <para/>
    /// NOTE: Model must be determined in the query calling methods
    /// </summary>
    public class GeneralRepository : GenericRepository
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GeneralRepository() { }
    }

    /// <summary>
    /// Implements a repository for a specific model
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class GeneralRepository<T> : GenericRepository<T> where T : class
    {
        /// <summary>
        /// Default constructor where tableName = target table in database <para/>
        /// NOTE: Only specify table name if needed
        /// </summary>
        /// <param name="tableName">target table</param>
        public GeneralRepository(string tableName = null) 
            : base(tableName) { }
    }
}
