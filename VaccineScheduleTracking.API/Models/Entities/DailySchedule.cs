using VaccineScheduleTracking.API.Models.DTOs;

namespace VaccineScheduleTracking.API.Models.Entities
{
    public class DailySchedule
    {
        public int DailyScheduleID { get; set; } 
        public DateTime Date { get; set; }// ngày làm việc
        
        public int Slot { get; set; }// Slot trong ngày (1-20)
        public TimeOnly StartTime => SetStartTime(Slot);
        public int? AppointmentID { get; set; }
        public Appointment Appointment { get; set; }


        private TimeOnly SetStartTime(int slot)
        {
            TimeOnly baseStartTime = new TimeOnly(7, 0);
            return baseStartTime.AddMinutes((slot - 1) * 45);
        }
    }
}
