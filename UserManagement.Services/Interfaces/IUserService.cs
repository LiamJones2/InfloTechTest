using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task<IEnumerable<User>> FilterByActive(bool isActive);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> CheckIfUserExists(long id);
    Task AddNewUser(User user);
    Task DeleteUser(User user);
    Task EditUser(User user);
}
