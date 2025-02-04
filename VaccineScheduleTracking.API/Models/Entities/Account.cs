namespace VaccineScheduleTracking.API.Models.Entities
{
    public class Account
    {
        public int AccountID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public string Status { get; set; }

        public Parent? Parent { get; set; }
        public Doctor? Doctor { get; set; }
        public Staff? Staff { get; set; }

        
    }
}
