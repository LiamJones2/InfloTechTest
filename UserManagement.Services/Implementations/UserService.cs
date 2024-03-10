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

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public User? CheckIfUserExists(long id) => _dataAccess.GetUserById<User>(id).FirstOrDefault();

    public void AddNewUser(User user) => _dataAccess.Create(user);

    public void DeleteUser(User user) => _dataAccess.Delete(user);

    public void EditUser(User user) => _dataAccess.Update(user);
}
