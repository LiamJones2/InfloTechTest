using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Data.TestData;
using System.Threading.Tasks;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    // If database is missing data then we take the testdata and fill it into the database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().Property(u => u.DateOfBirth)
            .HasConversion(
                v => v.HasValue ? v.Value.ToString("yyyy-MM-dd") : null,
                v => string.IsNullOrWhiteSpace(v) ? (DateOnly?)null : DateOnly.Parse(v));

        modelBuilder.Entity<User>().HasData(UserTestData.GetUserArray());
        modelBuilder.Entity<Log>().HasData(LogTestData.GetLogArray());
    }

    // Deletes all data from Logs and Users tables and then inserts our test data
    public async Task ResetDatabaseAsync()
    {
        Users!.RemoveRange(Users);
        Logs!.RemoveRange(Logs);

        var users = UserTestData.GetUserArray().Select(u => new User { Id = u.Id, Forename = u.Forename, Surname = u.Surname, Email = u.Email, DateOfBirth = u.DateOfBirth, IsActive = u.IsActive }).ToList();
        var logs = LogTestData.GetLogArray().Select(l => new Log {  Id = l.Id, UserId = l.UserId, CreatedAt = l.CreatedAt, Type = l.Type, Changes = l.Changes }).ToList();

        Users!.AddRange(users);
        Logs!.AddRange(logs);


        await SaveChangesAsync();
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Log>? Logs { get; set; }

    // Filters users in database that have IsActive set to parameter isActive
    public async Task<List<User>> FilterUserByActiveAsync(bool isActive) => await Set<User>().Where(user => user.IsActive == isActive).ToListAsync();

    // Returns all users/logs from database
    public async Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class => await Set<TEntity>().ToListAsync();

    // Gets certain user by id from database
    public async Task<User?> GetUserByIdAsync(long id) => await Set<User>().FirstOrDefaultAsync(user => user.Id == id);

    // Filters logs in database that have Type set to parameter type
    public async Task<List<Log>> FilterLogByTypeAsync(string type) => await Set<Log>().Where(user => user.Type == type).ToListAsync();

    // Gets certain log by id from database
    public async Task<Log?> GetLogByIdAsync(long id) => await Set<Log>().FirstOrDefaultAsync(log => log.Id == id);

    // Returns all logs that have UserId matching parameter id
    public async Task<IEnumerable<Log>> GetAllUserLogsByIdAsync(long id) => await base.Set<Log>().Where(log => log.UserId == id).ToListAsync();

    // Adds user/log to database and returns it
    public async Task<TEntity> CreateEntityAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = base.Entry(entity);
        base.Add(entity);
        await SaveChangesAsync();

        entry.State = EntityState.Unchanged;

        return entry.Entity;
    }

    // Updates existing user/log in database
    public async Task<TEntity> UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = base.Entry(entity);
        base.Update(entity);
        await SaveChangesAsync();

        entry.State = EntityState.Unchanged;
        entry.State = EntityState.Detached;

        return entry.Entity;
    }

    // Deletes existing user/log in database
    public async Task<TEntity> DeleteEntityAsync<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = base.Entry(entity);
        base.Remove(entity);
        await SaveChangesAsync();

        entry.State = EntityState.Unchanged;

        return entry.Entity;
    }
}
