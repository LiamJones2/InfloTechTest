using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Services.Domain.Interfaces;
using System.Threading.Tasks;

namespace UserManagement.Services.Domain.Implementations;

public class LogService : ILogService
{
    private readonly IDataContext _dataAccess;
    public LogService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>

    // Gets all logs in database and returns them to controller
    public async Task<IEnumerable<Log>> GetAllLogs() => await _dataAccess.GetAllAsync<Log>();

    // Gets all logs in database that have the correct type
    public async Task<IEnumerable<Log>> FilterByType(string type) => await _dataAccess.FilterLogByTypeAsync(type);

    // Gets all logs in database for a particular user by id
    public async Task<IEnumerable<Log>> GetAllUserLogsById(long id) => await _dataAccess.GetAllUserLogsByIdAsync(id);

    // Gets a certain log by id and returns it to controller
    public async Task<Log?> CheckIfLogExists(long id) => await _dataAccess.GetLogByIdAsync(id);
}
