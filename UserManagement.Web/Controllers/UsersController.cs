using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
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

        var userViewModel = new UserListViewModel
        {
            Items = userItems.Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                IsActive = p.IsActive
            }).ToList()
        };

        return View(userViewModel);
    }
}
