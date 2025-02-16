using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class TimeSlot
    {
        public int TimeSlotID { get; set; }
        public TimeOnly StartTime { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public int SlotNumber { get; set; }
        public bool Available { get; set; }
    }
}
