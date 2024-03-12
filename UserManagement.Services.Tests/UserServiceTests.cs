using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Data.TestData;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    private IDataContext _dataContext;
    private IUserService _userService;


    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseSqlServer("DevelopmentConnection")
        .Options;

        _dataContext = new DataContext(options);
        _userService = new UserService(_dataContext);
    }

    private async Task CreateContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseSqlServer("DevelopmentConnection")
        .Options;

        _dataContext = new DataContext(options);
        _userService = new UserService(_dataContext);
        await _dataContext.ResetDatabase();
    }


    [Fact]
    public async Task GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        var users = UserTestData.GetUserArray();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _userService.GetAllUsersAsync();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task FilterByActive_WhenActive_MustReturnOnlyActiveEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _userService.FilterByActive(true);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().OnlyContain(log => log.IsActive == true);
    }

    [Fact]
    public async Task FilterByActive_WhenNonActive_MustReturnOnlyNonActiveEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _userService.FilterByActive(false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().OnlyContain(log => log.IsActive == false);
    }

    [Fact]
    public async Task CheckIfUserExists_ExistingUser_ReturnsUser()
    {
        // Arrange
        CreateContext().Wait();
        long userId = 1;

        // Act
        var result = await _userService.CheckIfUserExists(userId);

        // Assert
        result.Should().BeOfType<User>();
    }

    [Fact]
    public async Task CheckIfUserExists_NonExistingUser_ReturnsNull()
    {
        CreateContext().Wait();
        // Arrange
        long userId = 100;

        // Act
        var result = await _userService.CheckIfUserExists(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddNewUser_GivenCorrectInfo_MustReturnTrueAndUserExists()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        var user = new User
        {
            Forename = "John",
            Surname = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1),
            IsActive = true
        };

        // Act: Invokes the method under test with the arranged parameters.
        await _userService.AddNewUser(user);

        var users = await _userService.GetAllUsersAsync();

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().Contain(user);
    }

    [Fact]
    public async Task AddNewUser_MissingEmail_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
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
            await _userService.AddNewUser(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public async Task AddNewUser_MissingForename_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
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
            await _userService.AddNewUser(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public async Task AddNewUser_MissingSurname_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
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
            await _userService.AddNewUser(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public async Task AddNewUser_MissingDateOfBirth_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
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
            await _userService.AddNewUser(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }
}
