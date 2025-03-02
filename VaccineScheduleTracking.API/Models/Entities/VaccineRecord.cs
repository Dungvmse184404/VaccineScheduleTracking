using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class VaccineRecord
    {
        public int VaccineRecordID { get; set; }
        public int ChildID { get; set; }
        public int VaccineTypeID { get; set; }
        public int? VaccineID { get; set; }
        public int? AppointmentID { get; set; }
        public DateOnly Date {  get; set; }
        public string? Note { get; set; }
        public Child Child { get; set; }
        public VaccineType VaccineType { get; set; }
        public Vaccine? Vaccine { get; set; }   
        public Appointment? Appointment { get; set; }
    }
}
