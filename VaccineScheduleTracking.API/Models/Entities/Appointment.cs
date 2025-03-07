using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API.Models.Entities
{
    public class Appointment
    {
        public int AppointmentID { get; set; }

        public int ChildID { get; set; }
        public Child Child { get; set; }

        public int AccountID { get; set; }
        public Account Account { get; set; }
        public int VaccineID { get; set; }
        public Vaccine Vaccine { get; set; }

        public int TimeSlotID { get; set; }
        public TimeSlot? TimeSlots { get; set; }
        public string Status { get; set; }


    }
}
