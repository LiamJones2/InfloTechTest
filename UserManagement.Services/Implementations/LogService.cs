using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

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
    
    public IEnumerable<Log> GetAllLogs() => _dataAccess.GetAll<Log>();

    public IEnumerable<Log> FilterByType(string? type) => _dataAccess.GetAll<Log>().Where(log => log.Type == type);

    public IEnumerable<Log> GetAllUserLogsById(long id) => _dataAccess.GetAllUserLogsById(id);

    public Log? CheckIfLogExists(long id) => _dataAccess.GetLogById<Log>(id).FirstOrDefault();
}
