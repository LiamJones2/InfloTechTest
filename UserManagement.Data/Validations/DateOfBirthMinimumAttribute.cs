using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Data.Validations;
public class DateOfBirthMinimumAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateOnly dateOfBirth)
        {
            // Check if the date is not the default value (01/01/0001)
            if (dateOfBirth == default(DateOnly))
            {
                return new ValidationResult("Date of birth is required.");
            }

            // Adjust the desired threshold year as needed
            int thresholdYear = 1900;

            if (dateOfBirth.Year <= thresholdYear)
            {
                return new ValidationResult("Date of birth must be past the year 1900.");
            }
        }
        else
        {
            // Value is not a DateOnly, handle accordingly
            return new ValidationResult("Invalid date format.");
        }

        return ValidationResult.Success;
    }
}
