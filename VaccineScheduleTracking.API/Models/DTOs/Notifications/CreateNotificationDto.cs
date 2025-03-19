namespace VaccineScheduleTracking.API_Test.Models.DTOs.Notifications
{
    public class CreateNotificationDto
    {
        
        public int AccountID { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public string? Footer { get; set; }
        public DateTime? MailSentDate { get; set; }
    }
}
