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
    /// Get a list of items
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    Task<List<User>> FilterUserByActiveAsync(bool isActive);

    Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class;

    Task<IEnumerable<Log>> GetAllUserLogsById(long id);

    Task<User?> GetUserById(long id);

    Task<List<Log>> FilterLogByTypeAsync(string type);

    Task<Log?> GetLogById(long id);

    Task ResetDatabase();

    /// <summary>
    /// Create a new item
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> Create<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Uodate an existing item matching the ID
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity> UpdateEntity<TEntity>(TEntity entity) where TEntity : class;

    Task<TEntity> Delete<TEntity>(TEntity entity) where TEntity : class;
}
