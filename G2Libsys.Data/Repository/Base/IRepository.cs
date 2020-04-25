using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace G2Libsys.Data.Repository
{
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
        public Task AddRange(IEnumerable<T> item);

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
        public Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get all items matching search
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetRangeAsync(string search);

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
        public Task RemoveAsync(T item);
    }
}
