using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Data.TestData;

namespace UserManagement.Data.Tests;

public class LogServiceTests
{
    private IDataContext _dataContext;
    private ILogService _logService;


    public LogServiceTests()
    {
        _dataContext = new DataContext();
        _logService = new LogService(_dataContext);
    }

    private void ResetContext()
    {
        _dataContext = new DataContext();
        _logService = new LogService(_dataContext);
    }

    [Fact]
    public void GetAllLogs_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext();
        IEnumerable<Log> logs = LogTestData.GetLogArray();

        // Act: Invokes the method under test with the arranged parameters.
        var result = _logService.GetAllLogs();

        // Assert: Verifies that the action of the method under test behaves as expected.
        logs.Should().HaveCountGreaterThan(0);
        foreach (var log in logs)
        {
            result.Should().ContainEquivalentOf(log);
        }
    }

    [Fact]
    public void FilterByType_WhenRequested_MustReturnOnlyCreatedTypeLogs()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext();
        var logs = LogTestData.GetLogArray();

        // Act: Invokes the method under test with the arranged parameters.
        var result = _logService.FilterByType("Created User");

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(logs.Where(u => u.Type == "Created User"));
    }

    [Fact]
    public void FilterByType_WhenRequested_MustReturnOnlyUpdatedTypeLogs()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext();
        var logs = LogTestData.GetLogArray();

        // Act: Invokes the method under test with the arranged parameters.
        var result = _logService.FilterByType("Updated User");

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(logs.Where(u => u.Type == "Updated User"));
    }

    [Fact]
    public void FilterByType_WhenRequested_MustReturnOnlyDeletedTypeLogs()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        ResetContext();
        var logs = LogTestData.GetLogArray();

        // Act: Invokes the method under test with the arranged parameters.
        var result = _logService.FilterByType("Deleted User");

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(logs.Where(u => u.Type == "Deleted User"));
    }
}
