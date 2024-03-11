using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Data;
using UserManagement.Web.Models.Logs;

namespace UserManagement.WebMS.Controllers;

public class LogsController : Controller
{
    private readonly ILogService _logService;
    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public ViewResult List(string? type)
    {
        IEnumerable<Log> logItems;

        if (type != null) logItems = _logService.FilterByType(type);
        else logItems = _logService.GetAllLogs();

        LogListViewModel userViewModel = new LogListViewModel
        {
            Items = logItems.Select(p => new Log
            {
                Id = p.Id,
                UserId = p.UserId,
                CreatedAt = p.CreatedAt,
                Type = p.Type,
                Changes = p.Changes,
            })
            .OrderBy(p => p.Id)
            .ToList()
        };

        return View(userViewModel);
    }

    [HttpGet]
    public ViewResult View(long id)
    {
        Log? log = _logService.CheckIfLogExists(id);

        if (log != null)
        {
            return View(log);
        }
        return View();
    }
}
