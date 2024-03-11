using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using UserManagement.Models;
using System.IO;
using System.Text.Json;
using System.Reflection;
using UserManagement.Data.TestData;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder model)
    {
        var users = UserTestData.GetUserArray();
        model.Entity<User>().HasData(users);

        var logs = LogTestData.GetLogArray();
        model.Entity<Log>().HasData(logs);
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Log>? Logs { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public IQueryable<User> GetUserById<TEntity>(long id) where TEntity : class => base.Set<User>().Where(user => user.Id == id);

    public IQueryable<Log> GetLogById<TEntity>(long id) where TEntity : class => base.Set<Log>().Where(log => log.Id == id);

    public IEnumerable<Log> GetAllUserLogsById(long id) => base.Set<Log>().Where(log => log.UserId == id);

    public TEntity Create<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = base.Entry(entity);
        base.Add(entity);
        SaveChanges();

        // Ensure that the entity is being tracked
        entry.State = EntityState.Unchanged;

        // Return the tracked entity
        return entry.Entity;
    }

    public TEntity UpdateEntity<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = base.Entry(entity);
        base.Update(entity);
        SaveChanges();

        // Ensure that the entity is being tracked
        entry.State = EntityState.Unchanged;

        // Return the tracked entity
        return entry.Entity;
    }

    public TEntity Delete<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = base.Entry(entity);
        base.Remove(entity);
        SaveChanges();

        // Ensure that the entity is being tracked
        entry.State = EntityState.Unchanged;

        // Return the tracked entity
        return entry.Entity;
    }

    public void SetupLogs(IEnumerable<Log> logs)
    {
        Logs?.AddRange(logs);
        SaveChanges();
    }
}
