using System;
using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API_Test.Helpers
{
    public class ValidDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateOnly date)
            {
                try
                {
                    DateTime dt = date.ToDateTime(new TimeOnly(0, 0));
                    return ValidationResult.Success;
                }
                catch (ArgumentOutOfRangeException)
                {
                    return new ValidationResult("Invalid date.");
                }
            }
            return new ValidationResult("Invalid date format.");
        }
    }
}
