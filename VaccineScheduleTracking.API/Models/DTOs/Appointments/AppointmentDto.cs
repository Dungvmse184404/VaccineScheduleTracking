using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Appointments
{
    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public string Child { get; set; }
        public string Doctor { get; set; }
        public int VaccineID { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; }


    }
}
