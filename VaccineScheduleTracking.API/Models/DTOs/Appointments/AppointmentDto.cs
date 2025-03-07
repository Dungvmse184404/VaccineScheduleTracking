using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Accounts;
using VaccineScheduleTracking.API_Test.Models.DTOs.Children;
using VaccineScheduleTracking.API_Test.Models.DTOs.Doctors;
using VaccineScheduleTracking.API_Test.Models.DTOs.Vaccines;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Appointments
{
    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public AppChildDto Child { get; set; }
        public AppDoctorDto Doctor { get; set; }
        public AppVaccineDto Vaccine { get; set; }
        public DateOnly Date { get; set; }
        public int SlotNumber { get; set; }
        public string Status {  get; set; }


    }
}
