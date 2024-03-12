using System.Collections.Generic;
using UserManagement.Data;
using System.Threading.Tasks;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task<IEnumerable<Log>> GetAllLogs();
    Task<IEnumerable<Log>> FilterByType(string type);
    Task<IEnumerable<Log>> GetAllUserLogsById(long id);
    Task<Log?> CheckIfLogExists(long id);
}
