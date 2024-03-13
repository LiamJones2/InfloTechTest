using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    private IDataContext _dataContext;
    private ILogService _logService;
    private IUserService _userService;
    private UsersController _controller;

    public UserControllerTests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase("UserManagement.Data.DataContext")
        .Options;

        _dataContext = new DataContext(options);

        _userService = new UserService(_dataContext);
        _logService = new LogService(_dataContext);
        _controller = new UsersController(_userService, _logService);
    }

    public async Task CreateContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase("UserManagement.Data.DataContext")
        .Options;

        _dataContext = new DataContext(options);

        _userService = new UserService(_dataContext);
        _logService = new LogService(_dataContext);
        _controller = new UsersController(_userService, _logService);

        await _dataContext.ResetDatabaseAsync();
    }

    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        var users = _dataContext.Users;

        // Act: Invokes the method under test with the arranged parameters.
        IActionResult result = await _controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotHaveCount(0);
        var viewResult = (ViewResult)result;
        viewResult.Model.Should().BeAssignableTo<UserListViewModel>();

        var userListViewModel = (UserListViewModel)viewResult.Model!;
        userListViewModel.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task List_WhenServiceReturnsActiveUsers_ModelMustContainOnlyActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        var users = _dataContext.Users;

        // Act: Invokes the method under test with the arranged parameters.
        IActionResult result = await _controller.List(isActive: true);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeOfType<ViewResult>();
        var viewResult = (ViewResult)result;
        viewResult.Model.Should().BeAssignableTo<UserListViewModel>();

        var userListViewModel = (UserListViewModel)viewResult.Model!;
        userListViewModel.Items.Should().OnlyContain(user => user.IsActive == true);
    }

    [Fact]
    public async Task List_WhenServiceReturnsNonActiveUsers_ModelMustContainOnlyNonActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        await CreateContext();

        // Act: Invokes the method under test with the arranged parameters.
        IActionResult result = await _controller.List(isActive: false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeOfType<ViewResult>();
        var viewResult = (ViewResult)result;
        viewResult.Model.Should().BeAssignableTo<UserListViewModel>();

        var userListViewModel = (UserListViewModel)viewResult.Model!;
        userListViewModel.Items.Should().OnlyContain(user => user.IsActive == false);
    }

    [Fact]
    public async Task PostNewUser_GivenCorrectModel_RedirectToList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        var users = _dataContext.Users;

        var validUserModel = new UserListItemViewModel
        {
            Forename = "Test",
            Surname = "Test",
            Email = "testuser@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        // Act: Invokes the method under test with the arranged parameters.
        var result = await _controller.PostNewUser(validUserModel);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeOfType<RedirectToActionResult>()
              .Which.ActionName.Should().Be("List");
    }

    [Fact]
    public async Task PostNewUser_MissingForename_RedirectBackToPageWithForenameIsRequiredError()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        var invalidUser = new UserListItemViewModel
        {
            Surname = "Test",
            Email = "testuser@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        // Act: Invokes the method under test with the arranged parameters.
        _controller.ModelState.AddModelError("Forename", "The Forename field is required.");
        var result = await _controller.PostNewUser(invalidUser);

        // Assert: Verifies that the action of the method under test behaves as expected.
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.ViewName.Should().Be("Add");
        viewResult.Model.Should().BeSameAs(invalidUser);
        _controller.ModelState.TryGetValue("Forename", out var modelStateEntry);

        // Assert that the ModelState has an entry for "Forename" with errors
        modelStateEntry.Should().NotBeNull();
        modelStateEntry!.Errors.Should().NotBeEmpty();

        // Assert that at least one error contains the expected error message
        modelStateEntry.Errors.Any(error => error.ErrorMessage.Contains("The Forename field is required."))
            .Should().BeTrue();
    }

    [Fact]
    public async Task PostNewUser_MissingSurname_RedirectBackToPageWithSurnameIsRequiredError()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        var invalidUser = new UserListItemViewModel
        {
            Forename = "Test",
            Email = "testuser@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        // Act: Invokes the method under test with the arranged parameters.
        _controller.ModelState.AddModelError("Surname", "The Surname field is required.");
        var result = await _controller.PostNewUser(invalidUser);

        // Assert: Verifies that the action of the method under test behaves as expected.
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.ViewName.Should().Be("Add");
        viewResult.Model.Should().BeSameAs(invalidUser);
        _controller.ModelState.TryGetValue("Surname", out var modelStateEntry);

        // Assert that the ModelState has an entry for "Forename" with errors
        modelStateEntry.Should().NotBeNull();
        modelStateEntry!.Errors.Should().NotBeEmpty();

        // Assert that at least one error contains the expected error message
        modelStateEntry.Errors.Any(error => error.ErrorMessage.Contains("The Surname field is required."))
            .Should().BeTrue();
    }

    [Fact]
    public async Task PostNewUser_MissingEmail_RedirectBackToPageWithEmailIsRequiredError()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        CreateContext().Wait();
        var invalidUser = new UserListItemViewModel
        {
            Forename = "Test",
            Surname = "Test",
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        // Act: Invokes the method under test with the arranged parameters.
        _controller.ModelState.AddModelError("Email", "The Email Address field is required.");
        var result = await _controller.PostNewUser(invalidUser);

        // Assert: Verifies that the action of the method under test behaves as expected.
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        viewResult.ViewName.Should().Be("Add");
        viewResult.Model.Should().BeSameAs(invalidUser);
        _controller.ModelState.TryGetValue("Email", out var modelStateEntry);

        // Assert that the ModelState has an entry for "Forename" with errors
        modelStateEntry.Should().NotBeNull();
        modelStateEntry!.Errors.Should().NotBeEmpty();

        // Assert that at least one error contains the expected error message
        modelStateEntry.Errors.Any(error => error.ErrorMessage.Contains("The Email Address field is required."))
            .Should().BeTrue();
    }
}
