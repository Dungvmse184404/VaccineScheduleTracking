using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.DTOs.Children
{
    public class ChildTimeSlotDto
    {
        public int ChildTimeSlotID { get; set; }
        public int ChildID { get; set; }
        public int SlotNumber { get; set; }
        public DailySchedule DailySchedule { get; set; }
        public bool Available { get; set; }
    }
}
