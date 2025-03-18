using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.Metadata;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Accounts
{
        public class RegisterAccountDto
        {
            [Required]
            public string Username { get; set; } = string.Empty;
            [Required]
            [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 kí tự.")]
            [RegularExpression(@"^(?=.*[A-Z])(?=.*\W).{8,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ viết hoa và 1 kí tự đặc biệt.")]
            public string Password { get; set; } = "";
            [Required]
            public string Firstname { get; set; } = string.Empty;
            [Required]
            public string Lastname { get; set; } = string.Empty;
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
            [Required]
            [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải có đúng 10 chữ số.")]
            public string PhoneNumber { get; set; } = string.Empty;
            public IFormFile? Avatar {  get; set; }
        }
}
