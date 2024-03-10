using System;
using System.ComponentModel.DataAnnotations;
using UserManagement.Data.Validations;

namespace UserManagement.Web.Models.Users;

public class UserListViewModel
{
    public List<UserListItemViewModel> Items { get; set; } = new();
}

public class UserListItemViewModel
{
    public long Id { get; set; }

    [Required]
    public string Forename { get; set; } = default!;

    [Required]
    public string Surname { get; set; } = default!;

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;

    [DateOfBirthMinimum]
    public DateOnly DateOfBirth { get; set; }

    public bool IsActive { get; set; }
}
