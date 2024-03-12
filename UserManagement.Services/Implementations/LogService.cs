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
    
    public async Task<IEnumerable<Log>> GetAllLogs() => await _dataAccess.GetAllAsync<Log>();

    public async Task<IEnumerable<Log>> FilterByType(string type) => await _dataAccess.FilterLogByTypeAsync(type);

    public async Task<IEnumerable<Log>> GetAllUserLogsById(long id) => await _dataAccess.GetAllUserLogsById(id);

    public async Task<Log?> CheckIfLogExists(long id) => await _dataAccess.GetLogById(id);
}
