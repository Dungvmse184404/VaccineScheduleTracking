namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class CancelReason
    {
        public int CancelReasonId { get; set; }
        public int AppointmentId { get; set; }
        public string Reason { get; set; }
    }
}
