using UserManagement.Data;

namespace UserManagement.Web.Models.Users;

public class UserDetailsViewModel : UserListItemViewModel
{
    public IEnumerable<Log> logs { get; set; } = new List<Log>();
}
