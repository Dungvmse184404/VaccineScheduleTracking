using System;

namespace VaccineScheduleTracking.API_Test.Models.Entities
{
    public class Announcement
    {
        public int AnnouncementID { get; set; }
        public int CreatedByAccountID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ToRole { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public DateTime MailSentDate { get; set; }
        public bool IsSent { get; set; }

    }
}
