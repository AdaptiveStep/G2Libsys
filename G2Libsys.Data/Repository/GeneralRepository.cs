namespace G2Libsys.Data.Repository
{
    /// <summary>
    /// <inheritdoc cref="GenericRepository"/>
    /// </summary>
    public class GeneralRepository : GenericRepository
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GeneralRepository() { }

    }

    /// <summary>
    /// <inheritdoc cref="GenericRepository{T}"/>
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    public class GeneralRepository<T> : GenericRepository<T> where T : class
    {
        /// <summary>
        /// Default constructor where tableName = target table in database <para/>
        /// Note: Only specify table name if needed
        /// </summary>
        /// <param name="tableName">target table</param>
        public GeneralRepository(string tableName = null) 
            : base(tableName) { }
    }
}
