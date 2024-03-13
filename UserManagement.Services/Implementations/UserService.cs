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
    
    // Returns all users with the parameter IsActive from database
    public async Task<IEnumerable<User>> FilterByActiveAsync(bool isActive) => await _dataAccess.FilterUserByActiveAsync(isActive);

    // Returns all users to the controller from database
    public async Task<IEnumerable<User>> GetAllUsersAsync() => await _dataAccess.GetAllAsync<User>();

    // Returns certain user by id from database
    public async Task<User?> CheckIfUserExistsAsync(long id) => await _dataAccess.GetUserByIdAsync(id);

    // Makes request to database to add new user
    public async Task AddNewUserAsync(User user)
    {
        var createdUser = await _dataAccess.CreateEntityAsync(user);

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

        await _dataAccess.CreateEntityAsync(createLog);
    }

    // Makes request to database to delete existing user
    public async Task DeleteUserAsync(User user) {
        var deletedUser = await _dataAccess.DeleteEntityAsync(user);

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

        await _dataAccess.CreateEntityAsync(deleteLog);
    }

    // Makes request to database to delete existing user
    public async Task EditUserAsync(User user)
    {
        var updatedUser = await _dataAccess.UpdateEntityAsync(user);

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

        await _dataAccess.CreateEntityAsync(updateLog);
    }
}
