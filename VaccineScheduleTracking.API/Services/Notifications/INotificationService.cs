using VaccineScheduleTracking.API_Test.Models.DTOs.Notifications;
using VaccineScheduleTracking.API_Test.Models.Entities;

namespace VaccineScheduleTracking.API_Test.Services.Notifications
{
    public interface INotificationService
    {
        Task<List<Notification>> GetAllNotificationAsync();
        Task<Notification> GetNotificationByIDAsync(int notificationId);
        Task<Notification> CreateNotification(CreateNotificationDto notification);
        Task<Notification> UpdateNotification(Notification notification);
        Task<Notification> DeleteNotification(Notification notification);
        Task SentAutoMailAnnouncementAsync(); 
    }
}
