namespace VaccineScheduleTracking.API_Test.Models.DTOs.Accounts
{
    public class UpdateAccountDto
    {
        public int AccountID { get; set; }
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
    }
}
