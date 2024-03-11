using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Data.TestData;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    private IDataContext _dataContext;
    private IUserService _userService;


    public UserServiceTests()
    {
        _dataContext = new DataContext();
        _userService = new UserService(_dataContext);
    }

    private void resetContext()
    {
        _dataContext = new DataContext();
        _userService = new UserService(_dataContext);
    }


    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        resetContext();
        var users = UserTestData.GetUserArray();

        // Act: Invokes the method under test with the arranged parameters.
        var result = _userService.GetAllUsers();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void FilterByActive_WhenActive_MustReturnOnlyActiveEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        resetContext();
        var users = _userService.GetAllUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = _userService.FilterByActive(true);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(users.Where(u => u.IsActive == true));
    }

    [Fact]
    public void FilterByActive_WhenNonActive_MustReturnOnlyNonActiveEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        resetContext();
        var users = _userService.GetAllUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = _userService.FilterByActive(false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(users.Where(u => u.IsActive == false));
    }

    [Fact]
    public void CheckIfUserExists_ExistingUser_ReturnsUser()
    {
        // Arrange
        resetContext();
        long userId = 1;

        // Act
        var result = _userService.CheckIfUserExists(userId);

        // Assert
        result.Should().BeOfType<User>();
    }

    [Fact]
    public void CheckIfUserExists_NonExistingUser_ReturnsNull()
    {
        resetContext();
        // Arrange
        long userId = 100;

        // Act
        var result = _userService.CheckIfUserExists(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AddNewUser_GivenCorrectInfo_MustReturnTrueAndUserExists()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        resetContext();
        var user = new User
        {
            Forename = "John",
            Surname = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1),
            IsActive = true
        };
        var users = _userService.GetAllUsers();

        // Act: Invokes the method under test with the arranged parameters.
        _userService.AddNewUser(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().Contain(user);
    }

    [Fact]
    public void AddNewUser_MissingEmail_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        resetContext();
        User user = new User
        {
            Forename = "Test",
            Surname = "Test",
            DateOfBirth = new DateOnly(1998, 12, 20),
            IsActive = true
        };
        var users = _userService.GetAllUsers();

        // Act: Invokes the method under test with the arranged parameters.

        try
        {
            _userService.AddNewUser(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public void AddNewUser_MissingForename_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        resetContext();
        User user = new User
        {
            Surname = "Test",
            Email = "Test@gmail.com",
            DateOfBirth = new DateOnly(1998, 12, 20),
            IsActive = true
        };
        var users = _userService.GetAllUsers();

        // Act: Invokes the method under test with the arranged parameters.

        try
        {
            _userService.AddNewUser(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public void AddNewUser_MissingSurname_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        resetContext();
        User user = new User
        {
            Forename = "Test",
            Email = "Test@gmail.com",
            DateOfBirth = new DateOnly(1998, 12, 20),
            IsActive = true
        };
        var users = _userService.GetAllUsers();

        // Act: Invokes the method under test with the arranged parameters.

        try
        {
            _userService.AddNewUser(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public void AddNewUser_MissingDateOfBirth_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        resetContext();
        User user = new User
        {
            Forename = "Test",
            Surname ="Test",
            Email = "Test@gmail.com",
            IsActive = true
        };
        var users = _userService.GetAllUsers();

        // Act: Invokes the method under test with the arranged parameters.

        try
        {
            _userService.AddNewUser(user);
        }
        catch (Exception) { }

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }
}
