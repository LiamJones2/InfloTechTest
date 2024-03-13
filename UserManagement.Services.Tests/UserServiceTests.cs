using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Data.TestData;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    private IDataContext _dataContext;
    private IUserService _userService;


    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase("UserManagement.Data.DataContext")
        .Options;

        _dataContext = new DataContext(options);
        _userService = new UserService(_dataContext);
    }

    private async Task ResetContext()
    {
        await _dataContext.ResetDatabaseAsync();
    }


    [Fact]
    public async Task GetAllAsync_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();
        var users = UserTestData.GetUserArray();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _userService.GetAllUsersAsync();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task FilterByActive_WhenActive_MustReturnOnlyActiveEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _userService.FilterByActiveAsync(true);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().OnlyContain(log => log.IsActive == true);
    }

    [Fact]
    public async Task FilterByActive_WhenNonActive_MustReturnOnlyNonActiveEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _userService.FilterByActiveAsync(false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().OnlyContain(log => log.IsActive == false);
    }

    [Fact]
    public async Task CheckIfUserExists_ExistingUser_ReturnsUser()
    {
        // Arrange
        ResetContext().Wait();
        var users = UserTestData.GetUserArray();
        long userId = 1;

        // Act
        var result = await _userService.CheckIfUserExistsAsync(userId);

        // Assert
        result.Should().BeOfType<User>();
        result.Should().BeEquivalentTo(users[0]);
    }

    [Fact]
    public async Task CheckIfUserExists_NonExistingUser_ReturnsNull()
    {
        ResetContext().Wait();
        // Arrange
        long userId = 100;

        // Act
        var result = await _userService.CheckIfUserExistsAsync(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddNewUser_GivenCorrectInfo_MustReturnTrueAndUserExists()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();
        var user = new User
        {
            Forename = "John",
            Surname = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1),
            IsActive = true
        };

        // Act: Invokes the method under test with the arranged parameters.
        await _userService.AddNewUserAsync(user);

        var users = await _userService.GetAllUsersAsync();

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().Contain(user);
    }

    [Fact]
    public async Task AddNewUser_MissingEmail_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();
        User user = new User
        {
            Forename = "Test",
            Surname = "Test",
            DateOfBirth = new DateOnly(1998, 12, 20),
            IsActive = true
        };
        var users = await _userService.GetAllUsersAsync();

        // Act: Invokes the method under test with the arranged parameters.

        try
        {
            await _userService.AddNewUserAsync(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public async Task AddNewUser_MissingForename_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();
        User user = new User
        {
            Surname = "Test",
            Email = "Test@gmail.com",
            DateOfBirth = new DateOnly(1998, 12, 20),
            IsActive = true
        };
        var users = await _userService.GetAllUsersAsync();

        // Act: Invokes the method under test with the arranged parameters.

        try
        {
            await _userService.AddNewUserAsync(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public async Task AddNewUser_MissingSurname_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();
        User user = new User
        {
            Forename = "Test",
            Email = "Test@gmail.com",
            DateOfBirth = new DateOnly(1998, 12, 20),
            IsActive = true
        };
        var users = await _userService.GetAllUsersAsync();

        // Act: Invokes the method under test with the arranged parameters.

        try
        {
            await _userService.AddNewUserAsync(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public async Task AddNewUser_MissingDateOfBirth_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();
        User user = new User
        {
            Forename = "Test",
            Surname ="Test",
            Email = "Test@gmail.com",
            IsActive = true
        };
        var users = await _userService.GetAllUsersAsync();

        // Act: Invokes the method under test with the arranged parameters.

        try
        {
            await _userService.AddNewUserAsync(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public async Task EditUser_WhenRequested_UserMustBeUpdated()
    {
        ResetContext().Wait();
        User? user = await _userService.CheckIfUserExistsAsync(1);
        string oldForename = "";
        if (user != null)
        {
            oldForename = user.Forename;
            user.Forename = "NewForename";
            await _userService.EditUserAsync(user);
        }
        else
        {
            throw new Exception("Error getting user");
        }

        // Act: Invokes the method under test with the arranged parameters.
        var users = await _userService.GetAllUsersAsync();

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.First<User>().Forename.Should().Be("NewForename");
        oldForename.Should().NotBe(users.First<User>().Forename);
    }

    [Fact]
    public async Task DeleteUser_WhenRequested_UserMustBeDeleted()
    {
        ResetContext().Wait();
        User? user = await _userService.CheckIfUserExistsAsync(1);
        if (user != null)
        {
            await _userService.DeleteUserAsync(user);
        }
        else
        {
            throw new Exception("Error getting user");
        }

        // Act: Invokes the method under test with the arranged parameters.
        var users = await _userService.GetAllUsersAsync();

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }
}
