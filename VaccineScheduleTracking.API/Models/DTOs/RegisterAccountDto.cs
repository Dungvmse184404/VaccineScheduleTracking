
using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API.Models.DTOs
{
    public class RegisterAccountDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MinLength(8, ErrorMessage = "Password must have at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\W).$",
        ErrorMessage = "Password must have at least 1 uppercase letter and 1 special character.")]
        public string Password { get; set; } = "";
        [Required]
        public string Firstname { get; set; } = string.Empty;
        [Required]
        public string Lastname { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Avatar { get; set; } = string.Empty;
    }
}
