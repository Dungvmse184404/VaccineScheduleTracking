using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Doctors
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string DoctorTimeSlot { get; set; }
        public AccountDto Account { get; set; }
    }
}
