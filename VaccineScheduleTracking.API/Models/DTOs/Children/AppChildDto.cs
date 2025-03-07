namespace VaccineScheduleTracking.API_Test.Models.DTOs.Children
{
    public class AppChildDto
    {
        public int ChildID { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
