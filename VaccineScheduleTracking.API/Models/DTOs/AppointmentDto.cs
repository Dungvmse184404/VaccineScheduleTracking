using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Models.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public string Child { get; set; }
        public string Doctor { get; set; }
        public string VaccineType { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; }

    }
}
