using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;


namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        IUserService service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void FilterByActive_WhenActive_MustReturnOnlyActiveEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        IUserService service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.FilterByActive(true);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(users.Where(u => u.IsActive == true));
    }

    [Fact]
    public void FilterByActive_WhenNonActive_MustReturnOnlyNonActiveEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        IUserService service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.FilterByActive(false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(users.Where(u => u.IsActive == false));
    }

    [Fact]
    public void CheckIfUserExists_ExistingUser_ReturnsUser()
    {
        // Arrange
        IUserService userService = CreateService();
        long userId = 1;
        var users = SetupUsers();

        // Act
        var result = userService.CheckIfUserExists(userId);

        // Assert
        result.Should().BeEquivalentTo(users.FirstOrDefault(u => u.Id == userId));
    }

    [Fact]
    public void CheckIfUserExists_NonExistingUser_ReturnsNull()
    {
        // Arrange
        long userId = 1;

        var dataAccessMock = new Mock<IDataContext>();
        dataAccessMock.Setup(m => m.GetUserById<User>(userId)).Returns(new User[0].AsQueryable());

        var userService = new UserService(dataAccessMock.Object);

        // Act
        var result = userService.CheckIfUserExists(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AddNewUser_GivenCorrectInfo_MustReturnTrueAndUserExists()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        IUserService service = CreateService();
        var user = new User
        {
            Id = 1,
            Forename = "John",
            Surname = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1),
            IsActive = true
        };

        // Act: Invokes the method under test with the arranged parameters.
        service.AddNewUser(user);

        // Assert: Verifies that the action of the method under test behaves as expected.
        User? createdUser = service.CheckIfUserExists(1);
        createdUser?.IsActive.Should().BeTrue();
        createdUser?.Should().BeEquivalentTo(user);
    }

    [Fact]
    public void AddNewUser_MissingEmail_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        IUserService service = CreateService();
        User user = new User
        {
            Forename = "Test",
            Surname = "Test",
            DateOfBirth = new DateOnly(1998, 12, 20),
            IsActive = true
        };

        // Act: Invokes the method under test with the arranged parameters.
        service.AddNewUser(user);
        var users = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public void AddNewUser_MissingForename_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        IUserService service = CreateService();
        User user = new User
        {
            Surname = "Test",
            Email = "Test@gmail.com",
            DateOfBirth = new DateOnly(1998, 12, 20),
            IsActive = true
        };

        // Act: Invokes the method under test with the arranged parameters.
        service.AddNewUser(user);
        var users = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public void AddNewUser_MissingSurname_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        IUserService service = CreateService();
        User user = new User
        {
            Forename = "Test",
            Email = "Test@gmail.com",
            DateOfBirth = new DateOnly(1998, 12, 20),
            IsActive = true
        };

        // Act: Invokes the method under test with the arranged parameters.
        service.AddNewUser(user);
        var users = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    [Fact]
    public void AddNewUser_MissingDateOfBirth_MustNotContainUser()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        IUserService service = CreateService();
        User user = new User
        {
            Forename = "Test",
            Surname = "Test",
            Email = "Test@gmail.com",
            IsActive = true
        };

        // Act: Invokes the method under test with the arranged parameters.
        service.AddNewUser(user);
        var users = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotContain(user);
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", string dateOfBirth = "1998-04-10", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                DateOfBirth = DateOnly.Parse(dateOfBirth),
                IsActive = isActive
            },
             new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                DateOfBirth = DateOnly.Parse(dateOfBirth),
                IsActive = false
            }
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new Mock<IDataContext>();
    private IUserService CreateService() => new UserService(_dataContext.Object);
}
