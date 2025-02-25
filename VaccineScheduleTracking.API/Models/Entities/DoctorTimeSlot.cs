using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class DoctorTimeSlot
    {
        public int DoctorTimeSlotID { get; set; }
        public int DoctorID { get; set; }
        public int SlotNumber { get; set; }
        public bool Available { get; set; }
        public int DailyScheduleID { get; set; }
        public DailySchedule DailySchedule { get; set; }


    }
}
