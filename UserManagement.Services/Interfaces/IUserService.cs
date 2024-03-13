using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Gets users by active state
    /// </summary>
    /// <typeparam name="bool"></typeparam>
    /// <param name="isActive"></param>
    /// <returns>Returns users with parameter isActive</returns>
    Task<IEnumerable<User>> FilterByActiveAsync(bool isActive);

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns>Returns all users</returns>
    Task<IEnumerable<User>> GetAllUsersAsync();

    /// <summary>
    /// Gets user that Id matches parameter id
    /// </summary>
    /// <typeparam name="long"></typeparam>
    /// <param name="id"></param>
    /// <returns>Returns user else returns null</returns>
    Task<User?> CheckIfUserExistsAsync(long id);

    /// <summary>
    /// Adds new user and log to database
    /// </summary>
    /// <typeparam name="User"></typeparam>
    /// <param name="user"></param>
    /// <returns></returns>
    Task AddNewUserAsync(User user);

    /// <summary>
    /// Deletes user and adds log to database
    /// </summary>
    /// <typeparam name="User"></typeparam>
    /// <param name="user"></param>
    /// <returns></returns>
    Task DeleteUserAsync(User user);

    /// <summary>
    /// Updates user and adds log to database
    /// </summary>
    /// <typeparam name="User"></typeparam>
    /// <param name="user"></param>
    /// <returns></returns>
    Task EditUserAsync(User user);
}
