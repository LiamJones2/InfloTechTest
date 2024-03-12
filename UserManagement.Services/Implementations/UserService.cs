using System;
using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using System.Threading.Tasks;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public async Task<IEnumerable<User>> FilterByActive(bool isActive) => await _dataAccess.FilterUserByActiveAsync(isActive);

    public async Task<IEnumerable<User>> GetAllUsersAsync() => await _dataAccess.GetAllAsync<User>();

    public async Task<User?> CheckIfUserExists(long id) => await _dataAccess.GetUserById(id);

    public async Task AddNewUser(User user)
    {
        var createdUser = await _dataAccess.Create(user);

        Log createLog = new Log
        {
            UserId = createdUser.Id,
            CreatedAt = DateTime.Now,
            Type = "Created User",
            Changes =
                $"Forename: {createdUser.Forename}<br>" +
                $"Surname: {createdUser.Surname}<br>" +
                $"Email: {createdUser.Email}<br>" +
                $"Date Of Birth: {createdUser.DateOfBirth:MM/dd/yyyy}"
    };

        await _dataAccess.Create(createLog);
    }


    public async Task DeleteUser(User user) {
        var deletedUser = await _dataAccess.Delete(user);

        Log deleteLog = new Log
        {
            UserId = deletedUser.Id,
            CreatedAt = DateTime.Now,
            Type = "Deleted User",
            Changes =
                $"Forename: {deletedUser.Forename}<br>" +
                $"Surname: {deletedUser.Surname}<br>" +
                $"Email: {deletedUser.Email}<br>" +
                $"Date Of Birth: {deletedUser.DateOfBirth:MM/dd/yyyy}"
        };

        await _dataAccess.Create(deleteLog);
    }

    public async Task EditUser(User user)
    {
        var updatedUser = await _dataAccess.UpdateEntity(user);

        Log updateLog = new Log
        {
            UserId = updatedUser.Id,
            CreatedAt = DateTime.Now,
            Type = "Updated User",

            Changes =
                $"Forename: {user.Forename} set to {updatedUser.Forename} <br>" +
                $"Surname: {user.Surname} set to {updatedUser.Surname} <br>" +
                $"Email: {user.Email} set to  {updatedUser.Email}  <br>" +
                $"Date Of Birth: {user.DateOfBirth:MM/dd/yyyy} set to  {updatedUser.DateOfBirth:MM/dd/yyyy}"
        };

        await _dataAccess.Create(updateLog);
    }
}
