using VaccineScheduleTracking.API.Repository.Notifications;
using VaccineScheduleTracking.API_Test.Models.DTOs;
using VaccineScheduleTracking.API_Test.Models.DTOs.Notifications;
using VaccineScheduleTracking.API_Test.Models.Entities;
using VaccineScheduleTracking.API_Test.Services.Accounts;
using static VaccineScheduleTracking.API_Test.Helpers.ValidationHelper;

namespace VaccineScheduleTracking.API_Test.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAccountService _accountService;
        public NotificationService(INotificationRepository notificationRepository, IAccountService accountService)
        {
            _notificationRepository = notificationRepository;
            _accountService = accountService;
        }

        public async Task<Notification> CreateNotification(CreateNotificationDto notification)
        {
            ValidateInput(notification, "chưa điền đủ thông tin notification");

            var notice = new Notification()
            {
                AccountID = notification.AccountID,
                CreatedDate = DateTime.Now,
                Topic = notification.Topic,
                Message = notification.Message,
                MailSentDate = notification.MailSentDate
            };
            return await _notificationRepository.AddNotification(notice);
        }

        

        public async Task<Notification> DeleteNotification(Notification notification)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Notification>> GetAllNotificationAsync()
        {
             return await _notificationRepository.GetAllNotificationAsync();
        }

        public async Task<Notification> GetNotificationByIDAsync(int notificationId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Notification>> GetPendingNotifications()
        {
            return await _notificationRepository.GetPendingNotifications();
        }

        public async Task SentMailNotificationAsync()
        {
            var notifications = await _notificationRepository.GetPendingNotifications();

            foreach (var noti in notifications)
            {
                var account = await _accountService.GetAccountByIdAsync(noti.AccountID);
                if (account != null)
                {
                    // send mail
                    //notification.MailSentDate = DateTime.Now;
                    //await _notificationRepository.UpdateNotification(notification);
                }
            }

        }

        public async Task<Notification> UpdateNotification(Notification notification)
        {
            throw new NotImplementedException();
        }

        
    }
}
