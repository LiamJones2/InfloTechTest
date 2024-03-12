using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using System.IO;
using UserManagement.Data.TestData;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DevelopmentConnection"));
    }

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

    public async Task ResetDatabase()
    {
        Database.ExecuteSqlRaw("DELETE FROM Logs");
        Database.ExecuteSqlRaw("DELETE FROM Users");

        Database.ExecuteSqlInterpolated($"DBCC CHECKIDENT('Users', RESEED, 0)");
        Database.ExecuteSqlInterpolated($"DBCC CHECKIDENT('Logs', RESEED, 0)");

        var users = UserTestData.GetUserArray().Select(u => new User { Forename = u.Forename, Surname = u.Surname, Email = u.Email, DateOfBirth = u.DateOfBirth, IsActive = u.IsActive }).ToList();
        var logs = LogTestData.GetLogArray().Select(l => new Log { UserId = l.UserId, CreatedAt = l.CreatedAt, Type = l.Type, Changes = l.Changes }).ToList();

        Users!.AddRange(users);
        Logs!.AddRange(logs);

        await SaveChangesAsync();
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Log>? Logs { get; set; }

    public async Task<List<User>> FilterUserByActiveAsync(bool isActive) => await Set<User>().Where(user => user.IsActive == isActive).ToListAsync();


    public async Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class => await Set<TEntity>().ToListAsync();

    public async Task<User?> GetUserById(long id) => await Set<User>().FirstOrDefaultAsync(user => user.Id == id);



    public async Task<List<Log>> FilterLogByTypeAsync(string type) => await Set<Log>().Where(user => user.Type == type).ToListAsync();

    public async Task<Log?> GetLogById(long id) => await Set<Log>().FirstOrDefaultAsync(log => log.Id == id);

    public async Task<IEnumerable<Log>> GetAllUserLogsById(long id) => await base.Set<Log>().Where(log => log.UserId == id).ToListAsync();

    public async Task<TEntity> Create<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = base.Entry(entity);
        base.Add(entity);
        await SaveChangesAsync();

        entry.State = EntityState.Unchanged;

        return entry.Entity;
    }

    public async Task<TEntity> UpdateEntity<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = base.Entry(entity);
        base.Update(entity);
        await SaveChangesAsync();

        entry.State = EntityState.Unchanged;

        return entry.Entity;
    }

    public async Task<TEntity> Delete<TEntity>(TEntity entity) where TEntity : class
    {
        var entry = base.Entry(entity);
        base.Remove(entity);
        await SaveChangesAsync();

        entry.State = EntityState.Unchanged;

        return entry.Entity;
    }
}
