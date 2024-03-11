using UserManagement.Data;

namespace UserManagement.Web.Models.Logs;

public class LogListViewModel
{
    public List<Log> Items { get; set; } = new();
}
