using VaccineScheduleTracking.API.Models.DTOs;

namespace VaccineScheduleTracking.API.Models.Entities
{
    public class Slot
    {
        public int SlotID { get; set; }
        public TimeSpan startTime { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int? AppointmentID { get; set; }
        public Appointment appointment { get; set; }

    }
}
