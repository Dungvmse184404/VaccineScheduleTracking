using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class ChildTimeSlot
    {
        public int ChildTimeSlotID { get; set; }
        public int ChildID { get; set; }
        public int SlotNumber { get; set; }
        public int DailyScheduleID { get; set; }
        public DailySchedule DailySchedule { get; set; }
        public bool Available { get; set; }
        
    }
}

