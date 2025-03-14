namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class CancelAppointment
    {
        public int CancelAppointmentID { get; set; }
        public int AppointmentID { get; set; }
        public DateTime CancelDate { get; set; }
        public string Reason { get; set; }
    }
}
