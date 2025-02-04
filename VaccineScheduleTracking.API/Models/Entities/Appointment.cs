namespace VaccineScheduleTracking.API.Models.Entities
{
    public class Appointment
    {
        public int AppointmentID { get; set; }

        public int ChildID { get; set; }
        public Child Child { get; set; }

        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public int VaccineTypeID { get; set; }
        public VaccineType VaccineType { get; set; }

        public DateTime Time { get; set; }
        public string Status{ get; set; }
        

    }
}
