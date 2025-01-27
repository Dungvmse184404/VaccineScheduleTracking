﻿namespace VaccineScheduleTracking.API.Models.DTOs
{
    public class AccountDto
    {
        public int AccountID { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public string Status { get; set; }
    }
}
