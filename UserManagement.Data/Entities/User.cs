﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserManagement.Data.Validations;

namespace UserManagement.Models;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long Id { get; set; }

    [Required]
    public string Forename { get; set; } = default!;

    [Required]
    public string Surname { get; set; } = default!;

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Date of Birth is required.")]
    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [DateOfBirthMinimum]
    public DateOnly? DateOfBirth { get; set; }

    public bool IsActive { get; set; }
}
