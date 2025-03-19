using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API.Models.Entities;

namespace VaccineScheduleTracking.API.Repository.Notifications
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllNotificationAsync();
        Task<Notification> GetNotificationByIDAsync(int notificationId);
        Task<Notification> AddNotification(Notification notification);
        Task<Notification> UpdateNotification(Notification notification);
        Task<Notification> DeleteNotification(Notification notification);
        Task<List<Notification>> GetPendingNotifications();

        // ------------------- Announcement -------------------
        Task<Announcement> AddAnnouncement(Announcement announce);
        Task<List<Announcement>> GetPendingAnnouncements();
        Task<List<Announcement>> GetAllAnnouncementAsync();
        Task AddAnnouncementRecipientAsync(AnnouncementRecipient announcement);
        Task<AnnouncementRecipient?> GetRecipientByAppointmentIDAsync(int appointmentId);
        Task AddAutoAnnouncementAsync(AutoAnnouncement autoAnnouncement);
        Task<AutoAnnouncement> GetAutoAnnounceAppointmentIDAsync(int appointmentId);
    }
}
