using System;
using System.Collections.Generic;
using System.Linq;
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
    IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

    IEnumerable<Log> GetAllUserLogsById(long id);

    IQueryable<User> GetUserById<TEntity>(long id) where TEntity : class;

    IQueryable<Log> GetLogById<TEntity>(long id) where TEntity : class;

    /// <summary>
    /// Create a new item
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    TEntity Create<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Uodate an existing item matching the ID
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    TEntity UpdateEntity<TEntity>(TEntity entity) where TEntity : class;

    TEntity Delete<TEntity>(TEntity entity) where TEntity : class;

    void SetupLogs(IEnumerable<Log> logs);
}
