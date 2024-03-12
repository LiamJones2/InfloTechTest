using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Data;
using UserManagement.Web.Models.Logs;
using System.Threading.Tasks;

namespace UserManagement.WebMS.Controllers;

public class LogsController : Controller
{
    private readonly ILogService _logService;
    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public async Task<ViewResult> List(string? type)
    {
        IEnumerable<Log> logItems;

        if (!string.IsNullOrEmpty(type)) logItems = await _logService.FilterByType(type);
        else logItems = await _logService.GetAllLogs();

        LogListViewModel logViewModel = new LogListViewModel
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

        return View(logViewModel);
    }

    [HttpGet]
    public async Task<ViewResult> View(long id)
    {
        Log? log = await _logService.CheckIfLogExists(id);

        if (log != null)
        {
            return View(log);
        }
        return View();
    }
}
