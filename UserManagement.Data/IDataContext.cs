using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public interface IDataContext
{
    public DbSet<User>? Users { get; set; }
    public DbSet<Log>? Logs { get; set; }

    /// <summary>
    /// Returns user that IsActive matches parameter isActive
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="isActive"></param>
    /// <returns>List of Users that match isActive</returns>
    Task<List<User>> FilterUserByActiveAsync(bool isActive);

    /// <summary>
    /// Get a list of items
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns>Returns list of items</returns>
    Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class;

    /// <summary>
    /// Returns logs that UserId matches id
    /// </summary>
    /// <typeparam name="long"></typeparam>
    /// <param name="id"></param>
    /// <returns>Returns logs that match id</returns>
    Task<IEnumerable<Log>> GetAllUserLogsByIdAsync(long id);

    /// <summary>
    /// Returns certain user in database by id
    /// </summary>
    /// <typeparam name="long"></typeparam>
    /// <param name="id"></param>
    /// <returns>Returns user if it exists else null</returns>
    Task<User?> GetUserByIdAsync(long id);

    /// <summary>
    /// Returns logs with Type matching parameter type
    /// </summary>
    /// <typeparam name="string"></typeparam>
    /// <param name="type"></param>
    /// <returns>Returns list of logs that match type</returns>
    Task<List<Log>> FilterLogByTypeAsync(string type);

    /// <summary>
    /// Returns log by id
    /// </summary>
    /// <typeparam name="long"></typeparam>
    /// <param name="id"></param>
    /// <returns>Log if it exists else null</returns>
    Task<Log?> GetLogByIdAsync(long id);

    /// <summary>
    /// Resets the database by clearing existing data, resetting identity seeds,
    /// and seeding the database with test data.
    /// </summary>
    /// <returns></returns>
    Task ResetDatabaseAsync();

    /// <summary>
    /// CreateEntityAsync a new item
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> CreateEntityAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Update an existing item matching the ID
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Delete an existing item matching the ID
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> DeleteEntityAsync<TEntity>(TEntity entity) where TEntity : class;
}
