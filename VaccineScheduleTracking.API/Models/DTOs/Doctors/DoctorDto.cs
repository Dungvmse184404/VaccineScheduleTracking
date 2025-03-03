using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Doctors
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public Account Account { get; set; }
        public string DoctorTimeSlot { get; set; }
    }
}
