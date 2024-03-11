using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

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
    public IEnumerable<User> FilterByActive(bool? isActive) => _dataAccess.GetAll<User>().Where(user => user.IsActive == isActive);

    public IEnumerable<User> GetAllUsers() => _dataAccess.GetAll<User>();

    public User? CheckIfUserExists(long id) => _dataAccess.GetUserById<User>(id).FirstOrDefault();

    public void AddNewUser(User user)
    {
        var createdUser = _dataAccess.Create(user);

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

        _dataAccess.Create(createLog);
    }


    public void DeleteUser(User user) {
        var deletedUser = _dataAccess.Delete(user);

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

        _dataAccess.Create(deleteLog);
    }

    public void EditUser(User user)
    {
        var updatedUser = _dataAccess.UpdateEntity(user);

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

        _dataAccess.Create(updateLog);
    }
}
