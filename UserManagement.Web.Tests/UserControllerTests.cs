using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    private readonly IDataContext _dataContextMock;
    private readonly IUserService _userService;
    private readonly UsersController _controller;

    public UserControllerTests()
    {
        _dataContextMock = new DataContext();


        _userService = new UserService(_dataContextMock);
        _controller = new UsersController(_userService);
    }

    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var users = _dataContextMock.Users;

        // Act: Invokes the method under test with the arranged parameters.
        var result = _controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotHaveCount(0);

        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void List_WhenServiceReturnsActiveUsers_ModelMustContainOnlyActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var users = _dataContextMock.Users;

        // Act: Invokes the method under test with the arranged parameters.
        var result = _controller.List(isActive: true);



        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotHaveCount(0);

        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().NotHaveCount(0);
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().OnlyContain(user => user.IsActive == true);
    }

    [Fact]
    public void List_WhenServiceReturnsNonActiveUsers_ModelMustContainOnlyNonActiveUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var users = _dataContextMock.Users;

        // Act: Invokes the method under test with the arranged parameters.
        var result = _controller.List(isActive: false);

        // Assert: Verifies that the action of the method under test behaves as expected.
        users.Should().NotHaveCount(0);

        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().OnlyContain(user => user.IsActive == false);
    }

    [Fact]
    public void PostNewUser_GivenCorrectModel_RedirectToList()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var users = _dataContextMock.Users;

        var validUserModel = new UserListItemViewModel
        {
            Forename = "Test",
            Surname = "Test",
            Email = "testuser@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        // Act: Invokes the method under test with the arranged parameters.
        var result = _controller.PostNewUser(validUserModel);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeOfType<RedirectToActionResult>()
              .Which.ActionName.Should().Be("List");
    }

    [Fact]
    public void PostNewUser_MissingForename_RedirectBackToPageWithForenameIsRequiredError()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var invalidUser = new UserListItemViewModel
        {
            Surname = "Test",
            Email = "testuser@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        // Act: Invokes the method under test with the arranged parameters.
        _controller.ModelState.AddModelError("Forename", "The Forename field is required.");
        var result = _controller.PostNewUser(invalidUser);

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
    public void PostNewUser_MissingSurname_RedirectBackToPageWithSurnameIsRequiredError()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var invalidUser = new UserListItemViewModel
        {
            Forename = "Test",
            Email = "testuser@example.com",
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        // Act: Invokes the method under test with the arranged parameters.
        _controller.ModelState.AddModelError("Surname", "The Surname field is required.");
        var result = _controller.PostNewUser(invalidUser);

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
    public void PostNewUser_MissingEmail_RedirectBackToPageWithEmailIsRequiredError()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var invalidUser = new UserListItemViewModel
        {
            Forename = "Test",
            Surname = "Test",
            DateOfBirth = new DateOnly(1990, 1, 1)
        };

        // Act: Invokes the method under test with the arranged parameters.
        _controller.ModelState.AddModelError("Email", "The Email Address field is required.");
        var result = _controller.PostNewUser(invalidUser);

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
