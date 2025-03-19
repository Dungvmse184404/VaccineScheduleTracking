using Microsoft.EntityFrameworkCore;
using VaccineScheduleTracking.API.Data;
using VaccineScheduleTracking.API.Repository.Notifications;
using VaccineScheduleTracking.API_Test.Helpers;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Repository.Doctors;

namespace VaccineScheduleTracking.API_Test.Repository.Notifications
{
    public class SQLNotificationRepository : INotificationRepository
    {

        private readonly VaccineScheduleDbContext _dbContext;

        public SQLNotificationRepository(VaccineScheduleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Notification> AddNotification(Notification notification)
        {
            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> DeleteNotification(Notification notification)
        {
            var result = await _dbContext.Notifications.FindAsync(notification.NotificationID);
            if (result != null)
            {
                _dbContext.Notifications.Remove(result);
                await _dbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<List<Notification>> GetAllNotificationAsync()
        {
            return await _dbContext.Notifications.ToListAsync();
        }


        public async Task<Notification> GetNotificationByIDAsync(int notificationId)
        {
            return await _dbContext.Notifications.FindAsync(notificationId);
        }

        public async Task<List<Notification>> GetPendingNotifications()
        {
            return await _dbContext.Notifications
                        .Where(n => n.MailSentDate != null && n.MailSentDate <= DateTime.Now)
                        .ToListAsync();
        }

        public async Task<Notification> UpdateNotification(Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
