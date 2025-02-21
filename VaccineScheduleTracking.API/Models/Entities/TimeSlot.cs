﻿using VaccineScheduleTracking.API.Models.Entities;

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



    }


}
