namespace VaccineScheduleTracking.API_Test.Models.DTOs.Notifications
{
    public class CreateAnnouncementDto
    {
        public int AccountID { get; set; }
        public string ToRole { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public string? Footer { get; set; }
        public DateTime? MailSentDate { get; set; }
    }
}
