namespace VaccineScheduleTracking.API_Test.Models.DTOs
{
    public class TimeSlotDto
    {
        public TimeOnly StartTime { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public int SlotNumber { get; set; }
        public bool Available { get; set; }
    }
}
