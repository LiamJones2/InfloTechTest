using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.Data;
using System.Threading.Tasks;

namespace UserManagement.WebMS.Controllers;

public class UsersController : Controller
{
    private readonly ILogService _logService;
    private readonly IUserService _userService;
    public UsersController(IUserService userService, ILogService logService)
    {
        _userService = userService;
        _logService = logService;
    }

    // When requested returns List view to client with User data depending on what isActive is
    // If isActive isn't null then it returns users with IsActive to that value
    [HttpGet]
    public async Task<IActionResult> List(bool? isActive = null)
    {

        IEnumerable<User> userItems;

        if (isActive.HasValue) userItems = await _userService.FilterByActiveAsync(isActive.GetValueOrDefault(false));
        else userItems = await _userService.GetAllUsersAsync();

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

    // When requested returns Add view
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    // When requested returns Edit view for a certain user if it exists else redirects to NotFound
    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        User? user = await _userService.CheckIfUserExistsAsync(id);

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
        return NotFound();
    }

    // When requested returns View view for a certain user if it exists else redirects to NotFound
    [HttpGet]
    public async Task<IActionResult> View(long id)
    {
        User? user = await _userService.CheckIfUserExistsAsync(id);

        if (user != null)
        {
            IEnumerable<Log> userLogs = await _logService.GetAllUserLogsById(id);

            UserDetailsViewModel modelUser = new UserDetailsViewModel
            {
                Id = user.Id,
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive,
                logs = userLogs
            };

            return View(modelUser);
        }
        return NotFound();
    }

    // When requested returns DeleteEntityAsync view for a certain user if it exists else redirects to NotFound
    [HttpGet]
    public async Task<IActionResult> Delete(long id)
    {
        User? user = await _userService.CheckIfUserExistsAsync(id);

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
        return NotFound();
    }

    // When requested by client, checks for correct data and if it is valid then we make a request to the _userService to add that new user to the database
    [HttpPost]
    public async Task<IActionResult> PostNewUser(UserListItemViewModel modelUser)
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

            await _userService.AddNewUserAsync(user);

            return RedirectToAction("List");
        }
        else
        {
            return View("Add", modelUser);
        }
    }

    // When requested by client, checks for correct data and if it is valid then we make a request to the _userService to add that edit an existing user in the database
    [HttpPost]
    public async Task<IActionResult> EditUser(UserListItemViewModel modelUser)
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

            await _userService.EditUserAsync(user);


            return RedirectToAction("List");
        }
        else
        {
            return View("Edit", modelUser);
        }
    }

    // When requested by client, checks for correct data and if it is valid then we make a request to the _userService to delete that existing user in the database
    [HttpPost]
    public async Task<IActionResult> DeleteUser(UserListItemViewModel modelUser)
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

            await _userService.DeleteUserAsync(user);

            return RedirectToAction("List");
        }

        return RedirectToAction("List");
    }
}
