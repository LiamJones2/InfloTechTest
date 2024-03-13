using System.Collections.Generic;
using UserManagement.Data;
using System.Threading.Tasks;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogService
{
    /// <summary>
    /// Gets all logs
    /// </summary>
    /// <param></param>
    /// <returns>Returns all logs</returns>
    Task<IEnumerable<Log>> GetAllLogs();

    /// <summary>
    /// Gets logs that Type matches parameter type
    /// </summary>
    /// <typeparam name="string"></typeparam>
    /// <param name="type"></param>
    /// <returns>Returns logs that match parameter type</returns>
    Task<IEnumerable<Log>> FilterByType(string type);

    /// <summary>
    /// Gets log that UserId matches parameter id
    /// </summary>
    /// <typeparam name="long"></typeparam>
    /// <param name="id"></param>
    /// <returns>Returns logs that match parameter id</returns>
    Task<IEnumerable<Log>> GetAllUserLogsById(long id);

    /// <summary>
    /// Gets log that Id matches parameter id if it exists
    /// </summary>
    /// <typeparam name="long"></typeparam>
    /// <param name="id"></param>
    /// <returns>Returns log that match parameter id else returns null</returns>
    Task<Log?> CheckIfLogExists(long id);
}
