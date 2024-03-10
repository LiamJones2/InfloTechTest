using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

    [HttpGet]
    public ViewResult List(bool? isActive = null)
    {
        IEnumerable<User> userItems;

        if (isActive.HasValue) userItems = _userService.FilterByActive(isActive.Value);
        else userItems = _userService.GetAll();

        UserListViewModel userViewModel = new UserListViewModel
        {
            Items = userItems.Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                DateOfBirth = p.DateOfBirth,
                IsActive = p.IsActive
            })
            .OrderBy(p => p.Id)
            .ToList()
        };

        return View(userViewModel);
    }

    [HttpGet]
    public ViewResult Add()
    {
        return View();
    }

    [HttpGet]
    public ViewResult Edit(long id)
    {
        User? user = _userService.CheckIfUserExists(id);

        if (user != null)
        {
            UserListItemViewModel modelUser = new UserListItemViewModel
            {
                Id = user.Id,
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive
            };

            return View(modelUser);
        }
        return View();
    }

    [HttpGet]
    public ViewResult View(long id)
    {
        User? user = _userService.CheckIfUserExists(id);

        if (user != null)
        {
            UserListItemViewModel modelUser = new UserListItemViewModel
            {
                Id = user.Id,
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive
            };

            return View(modelUser);
        }
        return View();
    }

    [HttpGet]
    public ViewResult Delete(long id)
    {
        User? user = _userService.CheckIfUserExists(id);

        if (user != null)
        {
            UserListItemViewModel modelUser = new UserListItemViewModel
            {
                Id = user.Id,
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive
            };

            return View(modelUser);
        }
        return View();
    }

    [HttpPost]
    public IActionResult PostNewUser(UserListItemViewModel modelUser)
    {
        if (ModelState.IsValid)
        {
            User user = new User
            {
                Forename = modelUser.Forename,
                Surname = modelUser.Surname,
                Email = modelUser.Email,
                DateOfBirth = modelUser.DateOfBirth,
                IsActive = true
            };

            _userService.AddNewUser(user);

            return RedirectToAction("List");
        }
        else
        {
            return View("Add", modelUser);
        }
    }

    [HttpPost]
    public IActionResult EditUser(UserListItemViewModel modelUser)
    {
        if (modelUser == null)
        {
            // Handle the case where modelUser is null, e.g., log an error or return a different result.
            return RedirectToAction("Error");
        }

        if (ModelState.IsValid)
        {
            User user = new User
            {
                Id = modelUser.Id,
                Forename = modelUser.Forename!,
                Surname = modelUser.Surname!,
                Email = modelUser.Email!,
                DateOfBirth = modelUser.DateOfBirth,
                IsActive = true
            };

            _userService.EditUser(user);


            return RedirectToAction("List");
        }
        else
        {
            return View("Edit", modelUser);
        }
    }

    [HttpPost]
    public IActionResult DeleteUser(UserListItemViewModel modelUser)
    {
        if (ModelState.IsValid)
        {
            User user = new User
            {
                Id = modelUser.Id,
                Forename = modelUser.Forename!,
                Surname = modelUser.Surname!,
                Email = modelUser.Email!,
                DateOfBirth = modelUser.DateOfBirth,
                IsActive = true
            };

            _userService.DeleteUser(user);

            return RedirectToAction("List");
        }

        return RedirectToAction("List");
    }
}
