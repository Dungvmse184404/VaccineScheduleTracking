using VaccineScheduleTracking.API_Test.Helpers;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Appointments
{
    public class ModifyAppointmentDto
    {
        public int AppointmentID { get; set; }
        public int VaccineID { get; set; }
        public int SlotNumber { get; set; }
        [ValidDate] 
         public DateOnly Date { get; set; }
    }
}
