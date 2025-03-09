using System.ComponentModel.DataAnnotations;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Accounts
{
    public class UpdateAccountDto
    {
        public int AccountID { get; set; }
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 kí tự.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\W).{8,}$",
        ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ viết hoa và 1 kí tự đặc biệt.")]
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải có đúng 10 chữ số.")]
        public string? PhoneNumber { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
