namespace VaccineScheduleTracking.API_Test.Models.DTOs.Children
{
    public class UpdateChildDto
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
}
