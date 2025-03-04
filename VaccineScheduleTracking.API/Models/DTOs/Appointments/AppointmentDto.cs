using VaccineScheduleTracking.API.Helpers;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API_Test.Models.DTOs.Children;
using VaccineScheduleTracking.API_Test.Models.DTOs.Doctors;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Appointments
{
    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public ChildDto Child { get; set; }
        public DoctorDto Doctor { get; set; }
        public int VaccineID { get; set; }
        public DateOnly Date { get; set; }
        public int SlotNumber { get; set; }
        public string Status {  get; set; }


    }
}
