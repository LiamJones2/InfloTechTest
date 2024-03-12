using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Data.TestData;
using System.Threading.Tasks;
using UserManagement.Models;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace UserManagement.Data.Tests;

public class LogServiceTests
{
    private IDataContext _dataContext;
    private ILogService _logService;
    private IUserService _userService;


    public LogServiceTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseSqlServer("DevelopmentConnection")
        .Options;

        _dataContext = new DataContext(options);
        _logService = new LogService(_dataContext);
        _userService = new UserService(_dataContext);
    }

    private async Task ResetContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseSqlServer("DevelopmentConnection")
        .Options;

        _dataContext = new DataContext(options);
        _logService = new LogService(_dataContext);
        _userService = new UserService(_dataContext);

        await _dataContext.ResetDatabase();
    }

    [Fact]
    public async Task GetAllLogs_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();
        IEnumerable<Log> logs = LogTestData.GetLogArray();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _logService.GetAllLogs();

        // Assert: Verifies that the action of the method under test behaves as expected.
        logs.Should().HaveCountGreaterThan(0);
        foreach (var log in logs)
        {
            result.Should().ContainEquivalentOf(log);
        }
    }

    [Fact]
    public async Task FilterByType_WhenRequested_MustReturnOnlyCreatedTypeLogs()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _logService.FilterByType("Created User");

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().OnlyContain(log => log.Type == "Created User");
    }

    [Fact]
    public async Task FilterByType_WhenRequested_MustReturnOnlyUpdatedTypeLogs()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _logService.FilterByType("Updated User");

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().OnlyContain(log => log.Type == "Updated User");
    }

    [Fact]
    public async Task FilterByType_WhenRequested_MustReturnOnlyDeletedTypeLogs()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _logService.FilterByType("Deleted User");

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().OnlyContain(log => log.Type == "Deleted User");
    }

    [Fact]
    public async Task GetAllLogs_AfterPostUser_CheckIfNewLogHasBeenMade()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();
        var oldLogs = await _logService.GetAllLogs();
        var user = new User
        {
            Forename = "Test",
            Surname = "Log",
            Email = "testlog@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1),
            IsActive = true
        };

        await _userService.AddNewUser(user);

        // Act: Invokes the method under test with the arranged parameters.
        var newLogs = await _logService.GetAllLogs();
        var result = newLogs.Last();

        // Assert: Verifies that the action of the method under test behaves as expected.
        newLogs.Count().Should().Be(oldLogs.Count() + 1);
        result.Type.Should().Be("Created User");
        result.Changes.Should().Contain($"Forename: {user.Forename}");
    }

    [Fact]
    public async Task GetAllLogs_AfterUpdateUser_CheckIfNewLogHasBeenMade()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();
        User? user = await _userService.CheckIfUserExists(1);
        var oldLogs = await _logService.GetAllLogs();
        string savedForename = "";
        if (user != null)
        {
            savedForename = user.Forename;
            user.Forename = "NewForename";
            await _userService.EditUser(user);
        }

        // Act: Invokes the method under test with the arranged parameters.
        var newLogs = await _logService.GetAllLogs();
        var users = await _userService.GetAllUsersAsync();
        var result = newLogs.Last();

        // Assert: Verifies that the action of the method under test behaves as expected.
        newLogs.Count().Should().Be(oldLogs.Count() + 1);
        result.Type.Should().Be("Updated User");
        result.Changes.Should().Contain($"Forename: NewForename");
        users.Should().NotContain(u => u.Forename == savedForename);
    }

    [Fact]
    public async Task GetAllLogs_AfterDeleteUser_CheckIfNewLogHasBeenMade()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext().Wait();
        User? user = await _userService.CheckIfUserExists(1);
        var oldLogs = await _logService.GetAllLogs();

        if (user != null)
        {
            await _userService.DeleteUser(user);
        }
        else
        {
            throw new Exception("Error getting user");
        }

        // Act: Invokes the method under test with the arranged parameters.
        var newLogs = await _logService.GetAllLogs();
        var result = newLogs.Last();

        // Assert: Verifies that the action of the method under test behaves as expected.
        newLogs.Count().Should().Be(oldLogs.Count() + 1);
        result.Type.Should().Be("Deleted User");
        result.Changes.Should().Contain(user.Forename);
    }
}
