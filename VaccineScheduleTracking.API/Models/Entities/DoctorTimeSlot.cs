using VaccineScheduleTracking.API.Models.Entities;
using static VaccineScheduleTracking.API_Test.Helpers.TimeSlotHelper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class DoctorTimeSlot
    {
        public int DoctorTimeSlotID { get; set; }
        public int DoctorID { get; set; }
        public int SlotNumber { get; set; }
        public string Weekdays => DailySchedule != null
         ? ConvertToWeekday(DailySchedule.AppointmentDate)
         : "Unknown";//<- unknow nếu DailySchedule.AppointmentDate null
        public bool Available { get; set; }
        public int DailyScheduleID { get; set; }
        public DailySchedule DailySchedule { get; set; }

        
    }
}
