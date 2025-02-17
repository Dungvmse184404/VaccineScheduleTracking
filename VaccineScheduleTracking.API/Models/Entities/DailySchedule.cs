using VaccineScheduleTracking.API.Models.DTOs;

namespace VaccineScheduleTracking.API.Models.Entities
{
    public class DailySchedule
    {
        public int DailyScheduleID { get; set; }
        public TimeOnly Date { get; set; }

    }
}
