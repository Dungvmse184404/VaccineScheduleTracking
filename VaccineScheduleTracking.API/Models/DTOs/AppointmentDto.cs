using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Models.DTOs
{
    public class AppointmentDto
    {
        public int? AppointmentID { get; set; }
        public int? ChildID { get; set; }
        public int? DoctorID { get; set; }
        public int? VaccineTypeID { get; set; }
        public DateTime? Time { get; set; }
        public string? Status { get; set; }
    }
}
