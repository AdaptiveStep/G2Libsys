namespace G2Libsys.Data.Repository
{
	using G2Libsys.Library;

	/// <summary>
	/// Required namespaces
	/// </summary>
	#region Namespaces
	using System.Collections.Generic;
    using System.Threading.Tasks;
    #endregion

    // Lägg inte till fler saker här, ändra bara 
    // befintliga metoder om ej fungerar.

    /// <summary>
    /// Repository with method type param
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Insert new item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> AddAsync<T>(T item);

        /// <summary>
        /// Insert multiple items
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task AddRangeAsync<T>(IEnumerable<T> item);

        /// <summary>
        /// Get item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> GetByIdAsync<T>(int id); 

        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetAllAsync<T>(int? id = null);

        /// <summary>
        /// Get all items matching search
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetRangeAsync<T>(string search);

        /// <summary>
        /// Get all items matching search with multiple filters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetRangeAsync<T>(T item);

        /// <summary>
        /// Update item in database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task UpdateAsync<T>(T item);

        /// <summary>
        /// Delete item from database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task RemoveAsync<T>(int id);

        /// <summary>
        /// Uses the AdvancedSearch stored procedure. Takes an Libraryobject that contains filtering parameters, 
        /// and gets all the Libraryobjects that match these conditions.
        /// For instance: if myBookobject.Title is "Harry" , then send myBookobject if you want all books that match that title.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<IEnumerable<LibraryObject>> AdvancedSearchAsync(LibraryObject paramsInObject);
	}

    /// <summary>
    /// Repository with type param
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Insert new item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<int> AddAsync(T item);

        /// <summary>
        /// Insert multiple items
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task AddRangeAsync(IEnumerable<T> item);

        /// <summary>
        /// Get item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetAllAsync(int? id = null);

        /// <summary>
        /// Get all items matching search
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetRangeAsync(string search);

        /// <summary>
        /// Get all items matching search with multiple filters
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetRangeAsync(T item);

        /// <summary>
        /// Update item in database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task UpdateAsync(T item);

        /// <summary>
        /// Delete item from database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task RemoveAsync(int id);

        /// <summary>
        /// Get a bunch of library objects by providing one libraryobject. The provided libraryobject contains desired attributes.
        /// </summary>
        /// <param name="paramsInObject"></param>
        /// <returns></returns>
        public Task<IEnumerable<LibraryObject>> AdvancedSearchAsync(LibraryObject paramsInObject);
    }
}
