using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using System.Threading.Tasks;
using System.Threading;

namespace UserManagement.Data.Tests;

public class DataContextTests
{
    private DataContext _dataContext;

    public DataContextTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase("UserManagement.Data.DataContext")
        .Options;

        _dataContext = new DataContext(options);
    }

    private async Task CreateContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase("UserManagement.Data.DataContext")
        .Options;

        _dataContext = new DataContext(options);

        await _dataContext.ResetDatabaseAsync();
    }

    [Fact]
    public async Task GetAll_GetAllUsers_MustNotBeEmpty()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _dataContext.GetAllAsync<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com",
            DateOfBirth = new DateOnly(2006,01,01)
        };
        await _dataContext.CreateEntityAsync(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _dataContext.GetAllAsync<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public async Task GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        DbSet<User>? users = _dataContext.Users;
        User? userToDelete = users?.FirstOrDefault();

        if (userToDelete != null)
        {
            // Detach the entity from the context
            _dataContext.Entry(userToDelete).State = EntityState.Detached;

            // DeleteEntityAsync the entity
            await _dataContext.DeleteEntityAsync(userToDelete);
        }

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _dataContext.GetAllAsync<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == userToDelete!.Email);

        CreateContext().Wait();
    }

    [Fact]
    public async Task GetAll_WhenEdited_UserForenameMustBeChanged()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        User? entity = await _dataContext.GetUserByIdAsync(1);
        var user = new User
        {
            Id = entity!.Id,
            Forename = "Changed Forename",
            Surname = entity.Surname,
            Email = entity.Email,
            DateOfBirth = entity.DateOfBirth,
        };

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _dataContext.UpdateEntityAsync(entity);

        result.Should().NotBeEquivalentTo(user);
        result.Forename.Should().NotBeEquivalentTo(user.Forename);

        CreateContext().Wait();
    }

    [Fact]
    public async Task GetUserById_FindUser_MustFindUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        var entity = await _dataContext.GetAllAsync<User>();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _dataContext.GetUserByIdAsync(entity[0].Id);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(entity[0]);
    }
}
