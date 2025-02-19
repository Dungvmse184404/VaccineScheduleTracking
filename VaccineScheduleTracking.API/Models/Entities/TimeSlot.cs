using VaccineScheduleTracking.API.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class TimeSlot
    {
        public int TimeSlotID { get; set; }
        public TimeOnly StartTime { get; set; }
        public int SlotNumber { get; set; }
        public bool Available { get; set; }
        public int DailyScheduleID { get; set; }
        public DailySchedule DailySchedule { get; set; }


        //public TimeSlot(int slotNumber)
        //{
        //    SlotNumber = slotNumber;
        //    StartTime = SetStartTime(slotNumber);
        //}

        //private TimeOnly SetStartTime(int slot)
        //{
        //    TimeOnly baseTime = new TimeOnly(7, 0);
        //    return baseTime.AddMinutes((slot - 1) * 45);
        //}


    }


}
