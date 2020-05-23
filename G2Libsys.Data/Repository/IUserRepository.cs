namespace G2Libsys.Data.Repository
{
    /// <summary>
    /// Required namespaces
    /// </summary>
    #region Namespaces
    using G2Libsys.Library;
    using System.Collections.Generic;
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
        public Task<User> VerifyLoginAsync(string username, string password);

        /// <summary>
        /// Implement specific user query
        /// </summary>
        public Task<bool> VerifyEmailAsync(string email);

        public Task<IEnumerable<Loan>> GetLoansAsync(int id);

        public Task<IEnumerable<LibraryObject>> GetLoanObjectsAsync(int id);

        ///// <summary>
        ///// Uses the AdvancedSearch stored procedure. Takes an Libraryobject that contains filtering parameters, 
        ///// and gets all the Libraryobjects that match these conditions.
        ///// For instance: if myBookobject.Title is "Harry" , then send myBookobject if you want all books that match that title.
        ///// </summary>
        ///// <param name="item"></param>
        ///// <returns></returns>
        //public Task<IEnumerable<LibraryObject>>
        //       AdvancedSearchAsync(LibraryObject paramsInObject);

    }
}
