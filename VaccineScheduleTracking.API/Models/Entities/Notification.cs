using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public int AccountID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public DateTime? MailSentDate { get; set; }

    }
}
