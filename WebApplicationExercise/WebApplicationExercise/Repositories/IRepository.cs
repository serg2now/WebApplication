using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebApplicationExercise.Repositories
{
    /// <summary>
    /// Repository interface for database table
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public interface IRepository<T> : IDisposable
    {
        /// <summary>
        /// Get Item by guid
        /// </summary>
        /// <param name="itemId">guid of item</param>
        /// <param name="loadItemsFromDb">flag defines should data be inserted in local collection</param>
        /// <returns>item from db</returns>
        T GetItem(Guid itemId, bool loadItemsFromDb = false);

        /// <summary>
        /// Get all items from database
        /// </summary>
        /// <param name="predicate">filter predicate</param>
        /// <param name="page">page number</param>
        /// <param name="offset">count of items to skip</param>
        /// <param name="sortString">sorting parameters</param>
        /// <returns>List of items</returns>
        List<T> GetItems(Expression<Func<T, bool>> predicate, int page, int offset, string sortString);

        /// <summary>
        /// Save item in database
        /// </summary>
        /// <param name="item">Entity item</param>
        /// <returns>result of saving, "" - all is ok, error message in case of errors</returns>
        string SaveItem(T item);

        /// <summary>
        /// Update existing item or add new
        /// </summary>
        /// <param name="newItem">Entity item</param>
        /// <returns>result of saving/updating, "" - all is ok, error message in case of errors</returns>
        string UpdateItem(T newItem);

        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="item">item for delete</param>
        /// <returns>result of deleting, "" - all is ok, error message in case of errors</returns>
        string DeleteItem(T item);
    }
}