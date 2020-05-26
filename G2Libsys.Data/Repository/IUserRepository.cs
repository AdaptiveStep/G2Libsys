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
        /// Verify user login credentials
        /// </summary>
        public Task<User> VerifyLoginAsync(string username, string password);

        /// <summary>
        /// Verify the user email
        /// </summary>
        public Task<bool> VerifyEmailAsync(string email);

        /// <summary>
        /// Get user loans
        /// </summary>
        public Task<IEnumerable<Loan>> GetLoansAsync(int id);

        /// <summary>
        /// Get user loanobjects
        /// </summary>
        public Task<IEnumerable<LibraryObject>> GetLoanObjectsAsync(int id);
    }
}
