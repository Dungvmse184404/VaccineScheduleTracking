namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class CancelReason
    {
        public int CancelReasonID { get; set; }
        public int AppointmentID { get; set; }
        public string Reason { get; set; }
    }
}
