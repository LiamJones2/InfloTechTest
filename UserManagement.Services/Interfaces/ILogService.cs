using System.Collections.Generic;
using UserManagement.Models;
using UserManagement.Data;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    IEnumerable<Log> GetAllLogs();
    IEnumerable<Log> FilterByType(string? type);
    IEnumerable<Log> GetAllUserLogsById(long id);
    Log? CheckIfLogExists(long id);
}
