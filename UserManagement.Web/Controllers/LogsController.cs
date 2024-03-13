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

    // When requested returns List view to client with Log data depending on what type is
    // If type isn't null then it returns logs with type to that value
    [HttpGet]
    public async Task<IActionResult> List(string? type)
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

    // When requested returns View view for a certain log if it exists else redirects to NotFound
    [HttpGet]
    public async Task<IActionResult> View(long id)
    {
        Log? log = await _logService.CheckIfLogExists(id);

        if (log != null)
        {
            return View(log);
        }
        return NotFound();
    }
}
