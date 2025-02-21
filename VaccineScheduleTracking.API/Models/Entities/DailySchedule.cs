using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API.Models.Entities
{
    public class DailySchedule
    {
        public int DailyScheduleID { get; set; }
        public DateOnly AppointmentDate { get; set; }

    }
}
